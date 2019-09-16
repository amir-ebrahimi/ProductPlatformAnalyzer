using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAEX_ClassModel;
using AMLEngineExtensions;
using CAEX_ClassModel.Validation;
using System.Reflection;

namespace ProductPlatformAnalyzer
{
    class AMLConverter
    {
        // static fields
        public static string IE_NAME = "ProductPlatform";
        public static string ROLE_LIB = IE_NAME + "RoleClassLib";

        // private fields and initializations
        private CAEXDocument document = null;
        private CAEXTables tables = null;

        // data structures to be populated from the document
        private HashSet<VariantGroup> variantGroups = new HashSet<VariantGroup>();
        private HashSet<Variant> variants = new HashSet<Variant>();
        private HashSet<string> constraints = new HashSet<string>();
        private HashSet<Resource> resources = new HashSet<Resource>();
        private HashSet<Trait> traits = new HashSet<Trait>();
        private HashSet<Operation> operations = new HashSet<Operation>();
        //private HashSet<partOperations> variantOperations = new HashSet<partOperations>();

        private Z3Solver cZ3Solver;

        // auxiliary data structures
        private Dictionary<string, Variant> id2VariantMappings
            = new Dictionary<string, Variant>();
        private Dictionary<string, Operation> id2OperationMappings
            = new Dictionary<string, Operation>();
        private Dictionary<string, Trait> id2TraitMappings
            = new Dictionary<string, Trait>();

        // constructor
        public AMLConverter(CAEXDocument caex_document, Z3Solver pZ3Solver) {
            document = caex_document;
            tables = document.Tables;
            cZ3Solver = pZ3Solver;
        }

        public void PopulateFrameworkWrapper(FrameworkWrapper pFrameworkWrapper)
        {
            pFrameworkWrapper.VariantSet = variants;
            pFrameworkWrapper.VariantGroupSet = variantGroups;
            pFrameworkWrapper.ConstraintSet = constraints;
            pFrameworkWrapper.OperationSet = operations;
            //pFrameworkWrapper.PartsOperationsSet = variantOperations;
            pFrameworkWrapper.ResourceSet = resources;
            pFrameworkWrapper.TraitSet = traits;
        }

        // populate the data staructures
        public void Populate()
        {
            // update the tables
            tables.UpdateAllTables();

            // get the user-defined role class types
            var roleClassNames = new List<string> {
                "Variant", "VariantGroup",
                "Operation", "OperationGroup",
                "Constraint", "Trait", "Resource",
            };

            var pathTable = tables.PathTable;
            var roleClassTypes = roleClassNames.ToDictionary(
                r => r,
                r => pathTable.CaexElement(PathJoin(ROLE_LIB, r)) as RoleClassType);

            // dispatch to functions to handle role classes
            Type type = this.GetType();
            foreach (var item in roleClassTypes)
            {
                var roleClassName = item.Key;
                var roleClassType = item.Value;
                MethodInfo method = type.GetMethod("Populate" + roleClassName);
                method.Invoke(this, new object[] { roleClassName, roleClassType });
            }
        }

        public void PopulateVariant(string role, RoleClassType type)
        {      
            int indexCounter = 1;
            foreach (var roleRef in tables.RegisteredReferences(type))
            {
                var parent = roleRef.GetParent();
                if (parent is InternalElementType)
                {
                    // create a Variantand set fields
                    Variant tempVariant = new Variant
                    {
                        Names = parent.Name(),
                        //index = indexCounter++
                    };
                    variants.Add(tempVariant);
                    // store ID and variant
                    var ie = parent as InternalElementType;
                    id2VariantMappings.Add(ie.ID.Value, tempVariant);
                }
            }
        }

        public void PopulateVariantGroup(string role, RoleClassType type)
        {
            foreach (var roleRef in tables.RegisteredReferences(type))
            {
                var parent = roleRef.GetParent();
                if (parent is InternalElementType)
                {
                    var ie = parent as InternalElementType;
                    var cardinality = ie.GetAttributeValue("groupCardinality");
                    List<CAEXObject> elements = new List<CAEXObject>();
                    ie.GetInternalElementsAndExternalInterfaces(elements);
                    List<Variant> variants = new List<Variant>();
                    foreach (InternalElementType e in elements)
                    {
                        if(!e.Name().Equals(parent.Name()))
                        {
                            var refID = e.GetRefBaseSystemUnitPath();
                            variants.Add(id2VariantMappings[refID]);
                        }
                    }
                    // create a VariantGroup and set fields
                    VariantGroup tempVariantGroup = new VariantGroup
                    {
                        Names = parent.Name(),
                        GCardinality = cardinality,
                        Variants = variants
                    };
                    variantGroups.Add(tempVariantGroup);
                }
            }
        }

        public void PopulateOperation(string role, RoleClassType type)
        {
            var reqSeparator = ';';
            foreach (var roleRef in tables.RegisteredReferences(type))
            {
                var parent = roleRef.GetParent();
                if (parent is InternalElementType)
                {
                    var ie = parent as InternalElementType;
                    var pre  = ie.GetAttributeValue("precondition");
                    var pos = ie.GetAttributeValue("postcondition");
                    //var req  = ie.GetAttributeValue("requirements").Split(reqSeparator);
                    var req = ie.GetAttributeValue("requirements");
                    var trigger = ie.GetAttributeValue("trigger");
                    var resource = ie.GetAttributeValue("resource");
                    // create an operation and set fields

                    Operation tempOperation = new Operation(parent.Name()
                                                            , trigger
                                                            , req
                                                            , pre
                                                            , pos
                                                            , resource);
                    
                    /*tempOperation.Name = parent.Name();
                    //tempOperation.displayName = parent.Name();
                    if (pre.ToString() != "")
                        tempOperation.Precondition = new List<string>(new string[] { pre });
                    if (pos.ToString() != "")
                        tempOperation.postcondition = new HashSet<string>(new string[] { pos });
                    if (req.Count() > 0)
                        if (req[0] != "")
                            tempOperation.Requirement = req;*/

                    operations.Add(tempOperation);
                    // store ID and operation
                    id2OperationMappings.Add(ie.ID.Value, tempOperation);
                }
            }
        }

        /*public void PopulateOperationGroup(string role, RoleClassType type)
        {
            foreach (var roleRef in tables.RegisteredReferences(type))
            {
                var parent = roleRef.GetParent();
                if (parent is InternalElementType)
                {
                    var variantName = (parent.GetParent() as InternalElementType).Name();
                    var elements = new List<CAEXObject>();
                    var ie = parent as InternalElementType;
                    ie.GetInternalElementsAndExternalInterfaces(elements);
                    List<Operation> operations = new List<Operation>();
                    foreach (InternalElementType e in elements)
                    {
                        if (!e.Name().Equals(parent.Name()))
                        {
                            var refID = e.GetRefBaseSystemUnitPath();
                            operations.Add(id2OperationMappings[refID]);
                        }
                    }
                    // create a variantOperation and set field
                    partOperations tempVariantOperations = new partOperations();
                    tempVariantOperations.setPartExpr(variantName);
                    tempVariantOperations.setOperations(operations);
                    variantOperations.Add(tempVariantOperations);
                }
            }
        }*/

        public void PopulateConstraint(string role, RoleClassType type)
        {
            foreach (var roleRef in tables.RegisteredReferences(type))
            {
                var parent = roleRef.GetParent();
                if (parent is InternalElementType)
                {
                    var ie = parent as InternalElementType;
                    constraints.Add(ie.GetAttributeValue("logic"));
                }
            }
        }

        public void PopulateTrait(string role, RoleClassType type)
        {
            foreach (var roleRef in tables.RegisteredReferences(type))
            {
                var parent = roleRef.GetParent();
                if (parent is InternalElementType)
                {
                    HashSet<Tuple<string, string>> attributesField =
                        new HashSet<Tuple<string, string>>();
                    var traitName = parent.Name();
                    var ie = parent as InternalElementType;
                    foreach (var attr in ie.Attributes())
                    {
                        var attrName = attr.Name();
                        var attrType = attr.AttributeDataType.Value.Split(':')[1];
                        attributesField.Add(
                            new Tuple<string, string>(attrName, attrType));
                    }
                    // create a trait and set field
                    Trait tempTrait = new Trait
                    {
                        Names = traitName,
                        // In the AML modeling for now, trait cannot inherit trait
                        Inherit = new HashSet<Trait>(),
                        Attributes = attributesField
                    };
                    traits.Add(tempTrait);
                    // store ID and trait
                    id2TraitMappings.Add(ie.ID.Value, tempTrait);
                }
            }
        }

        public void PopulateResource(string role, RoleClassType type)
        {
            foreach (var roleRef in tables.RegisteredReferences(type))
            {
                var parent = roleRef.GetParent();
                if (parent is InternalElementType)
                {
                    var resourceName = parent.Name();
                    var displayName = resourceName;
                    HashSet<Tuple<string, string, string>> attributesField =
                        new HashSet<Tuple<string, string, string>>();
                    HashSet<Trait> traits = new HashSet<Trait>();

                    var ie = parent as InternalElementType;
                    Dictionary<string, string> traitAttr2value =
                        new Dictionary<string, string>();
                    foreach (var attr in ie.Attributes())
                        traitAttr2value.Add(attr.Name(), attr.Value);

                    foreach (InternalElementType t in ie.GetAllInternalElements())
                    {
                        var trait = id2TraitMappings[t.ID.Value];
                        traits.Add(trait);
                        foreach (var tuple in trait.Attributes)
                        {
                            if (traitAttr2value.ContainsKey(tuple.Item1))
                                attributesField.Add(
                                    new Tuple<string, string, string>(
                                        tuple.Item1, tuple.Item2, traitAttr2value[tuple.Item1]));
                        }             
                    }
                    // create a resource and set fields
                    Resource tempResource = new Resource
                    {
                        Name = resourceName,
                        //displayName = displayName,
                        //traits = traits,
                        //attributes = attributesField
                    };
                    resources.Add(tempResource);
                }
            }
        }

        // convinence function to build path
        public static string PathJoin(string dirName, string baseName)
        {
            return dirName + "/" + baseName;
        }
    }
}