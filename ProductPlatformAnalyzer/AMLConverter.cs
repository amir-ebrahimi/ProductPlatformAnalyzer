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
        private HashSet<variantGroup> variantGroups = new HashSet<variantGroup>();
        private HashSet<variant> variants = new HashSet<variant>();
        private HashSet<string> constraints = new HashSet<string>();
        private HashSet<resource> resources = new HashSet<resource>();
        private HashSet<trait> traits = new HashSet<trait>();
        private HashSet<operation> operations = new HashSet<operation>();
        private HashSet<partOperations> variantOperations = new HashSet<partOperations>();

        // auxiliary data structures
        private Dictionary<string, variant> id2VariantMappings
            = new Dictionary<string, variant>();
        private Dictionary<string, operation> id2OperationMappings
            = new Dictionary<string, operation>();
        private Dictionary<string, trait> id2TraitMappings
            = new Dictionary<string, trait>();

        // constructor
        public AMLConverter(CAEXDocument caex_document) {
            document = caex_document;
            tables = document.Tables;
        }

        public void PopulateFrameworkWrapper(FrameworkWrapper pFrameworkWrapper)
        {
            pFrameworkWrapper.VariantSet = variants;
            pFrameworkWrapper.VariantGroupSet = variantGroups;
            pFrameworkWrapper.ConstraintSet = constraints;
            pFrameworkWrapper.OperationSet = operations;
            pFrameworkWrapper.PartsOperationsSet = variantOperations;
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
                    // create a variant and set fields
                    variant tempVariant = new variant
                    {
                        names = parent.Name(),
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
                    HashSet<variant> variants = new HashSet<variant>();
                    foreach (InternalElementType e in elements)
                    {
                        if(!e.Name().Equals(parent.Name()))
                        {
                            var refID = e.GetRefBaseSystemUnitPath();
                            variants.Add(id2VariantMappings[refID]);
                        }
                    }
                    // create a variantGroup and set fields
                    variantGroup tempVariantGroup = new variantGroup
                    {
                        names = parent.Name(),
                        gCardinality = cardinality,
                        variants = variants
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
                    var req  = ie.GetAttributeValue("requirements").Split(reqSeparator);
                    // create an operation and set fields

                    operation tempOperation = new operation();
                    
                    tempOperation.names = parent.Name();
                    //tempOperation.displayName = parent.Name();
                    if (pre.ToString() != "")
                        tempOperation.precondition = new HashSet<string>(new string[] { pre });
                    /*if (pos.ToString() != "")
                        tempOperation.postcondition = new HashSet<string>(new string[] { pos });*/
                    if (req.Count() > 0)
                        if (req[0] != "")
                            tempOperation.requirements = new HashSet<string>(req);

                    operations.Add(tempOperation);
                    // store ID and operation
                    id2OperationMappings.Add(ie.ID.Value, tempOperation);
                }
            }
        }

        public void PopulateOperationGroup(string role, RoleClassType type)
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
                    HashSet<operation> operations = new HashSet<operation>();
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
        }

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
                    trait tempTrait = new trait
                    {
                        names = traitName,
                        // In the AML modeling for now, trait cannot inherit trait
                        inherit = new HashSet<trait>(),
                        attributes = attributesField
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
                    HashSet<trait> traits = new HashSet<trait>();

                    var ie = parent as InternalElementType;
                    Dictionary<string, string> traitAttr2value =
                        new Dictionary<string, string>();
                    foreach (var attr in ie.Attributes())
                        traitAttr2value.Add(attr.Name(), attr.Value);

                    foreach (InternalElementType t in ie.GetAllInternalElements())
                    {
                        var trait = id2TraitMappings[t.ID.Value];
                        traits.Add(trait);
                        foreach (var tuple in trait.attributes)
                        {
                            if (traitAttr2value.ContainsKey(tuple.Item1))
                                attributesField.Add(
                                    new Tuple<string, string, string>(
                                        tuple.Item1, tuple.Item2, traitAttr2value[tuple.Item1]));
                        }             
                    }
                    // create a resource and set fields
                    resource tempResource = new resource
                    {
                        names = resourceName,
                        //displayName = displayName,
                        traits = traits,
                        attributes = attributesField
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