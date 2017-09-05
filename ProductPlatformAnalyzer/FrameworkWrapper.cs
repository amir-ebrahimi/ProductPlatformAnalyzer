using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Xml;

namespace ProductPlatformAnalyzer
{
    public class FrameworkWrapper
    {
        private List<variantGroup> cVariantGroupList;
        private List<variant> cVariantList;
        private List<string> cConstraintList;
        private List<operation> cOperationList;
        private List<variantOperations> cVariantsOperationsList;
        private List<string> cActiveOperationInstanceNamesList;
        private List<virtualVariant2VariantExpr> cVirtualVariant2VariantExprList;
        private List<string> cActiveOperationNamesList;
        private List<string> cInActiveOperationNamesList;
        private List<string> cOperationInstanceList;
        private List<station> cStationList;
        private List<resource> cResourceList;
        private List<trait> cTraitList;
        private int cVirtualCounter;

        public List<variantGroup> VariantGroupList
        {
            get { return this.cVariantGroupList; }
            set { this.cVariantGroupList = value; }
        }

        public List<variant> VariantList
        {
            get { return this.cVariantList; }
            set { this.cVariantList = value; }
        }

        private int getNextVirtualIndex()
        {
            return cVirtualCounter++;
        }

        public List<string> ConstraintList
        {
            get { return this.cConstraintList; }
            set { this.cConstraintList = value; }
        }

        public List<operation> OperationList
        {
            get { return this.cOperationList; }
            set { this.cOperationList = value; }
        }

        public List<variantOperations> VariantsOperationsList
        {
            get { return this.cVariantsOperationsList; }
            set { this.cVariantsOperationsList = value; }
        }

        public List<string> ActiveOperationNamesList
        {
            get { return this.cActiveOperationNamesList; }
            set { this.cActiveOperationNamesList = value; }
        }

        public List<string> ActiveOperationInstanceNamesList
        {
            get { return this.cActiveOperationInstanceNamesList; }
            set { this.cActiveOperationInstanceNamesList = value; }
        }

        public List<string> InActiveOperationNamesList
        {
            get { return this.cInActiveOperationNamesList; }
            set { this.cInActiveOperationNamesList = value; }
        }

        public List<string> OperationInstanceList
        {
            get { return this.cOperationInstanceList; }
            set { this.cOperationInstanceList = value; }
        }

        public List<station> StationList
        {
            get { return this.cStationList; }
            set { this.cStationList = value; }
        }

        public List<resource> ResourceList
        {
            get { return this.cResourceList; }
            set { this.cResourceList = value; }
        }

        public List<trait> TraitList
        {
            get { return this.cTraitList; }
            set { this.cTraitList = value; }
        }

        public FrameworkWrapper()
        {
            cVariantList = new List<variant>();
            cVariantGroupList = new List<variantGroup>();
            cConstraintList = new List<string>();
            cOperationList = new List<operation>();
            cActiveOperationInstanceNamesList = new List<string>();
            cActiveOperationNamesList = new List<string>();
            cInActiveOperationNamesList = new List<string>();
            cOperationInstanceList = new List<string>();
            cVariantsOperationsList = new List<variantOperations>();
            cStationList = new List<station>();
            cResourceList = new List<resource>();
            cTraitList = new List<trait>();
            cVirtualVariant2VariantExprList = new List<virtualVariant2VariantExpr>();
            cVirtualCounter = 0;
        }

        /// <summary>
        /// Returns the number of operations
        /// </summary>
        /// <returns>number of operations</returns>
        public int getNumberOfOperations()
        {
            return OperationList.Count();
        }

        /// <summary>
        /// Returns the number of operations which are active, meaning they are related to a variant
        /// </summary>
        /// <returns>Number of active operations</returns>
        public int getNumberOfActiveOperations()
        {
            return cActiveOperationNamesList.Count();
        }

        /// <summary>
        /// This function returns a list of operation names
        /// </summary>
        /// <returns>List of operation names</returns>
        public List<string> getListOfOperationNames()
        {
            List<string> lOperationNames = new List<string>();
            try
            {
                foreach (operation lOperation in cOperationList)
                {
                    if (!lOperationNames.Contains(lOperation.names))
                        lOperationNames.Add(lOperation.names);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in getListOfOperationNames");
                Console.WriteLine(ex.Message);
            }
            return lOperationNames;
        }

        /// <summary>
        /// Returns the list of variant operations corresponding to a specific variant
        /// </summary>
        /// <param name="pVariantExpr">The variant which we want to return its variant operations</param>
        /// <returns>The list of variant operations corresponding to that variant</returns>
        public List<operation> getVariantExprOperations(string pVariantExpr)
        {
            List<operation> lResultOperations = null;
            try
            {
                foreach (variantOperations lVariantOperations in VariantsOperationsList)
                {
                    if (lVariantOperations.getVariantExpr().Equals(pVariantExpr))
                        lResultOperations = lVariantOperations.getOperations();
                }

                //TODO: fix the LINQ query for this
                /*List<operation> lOperations = (from variantsOperations in VariantsOperationsList
                                         where VariantsOperationsList.getVariantExpr() == pVariantExpr.Trim()
                                         select VariantsOperationsList.getOperations()).ToList();

                if (lOperations.Count >= 1)
                    lResultOperations = lOperations;*/

            }
            catch (Exception ex)
            {
                Console.WriteLine("error in getVariantExprOperations, pVariantExpr: " + pVariantExpr);
                Console.WriteLine(ex.Message);
            }
            return lResultOperations;
        }
        
        public operation findOperationWithName(String pOperationName)
        {
            operation tempResultOperation = null;
            try
            {
                foreach (operation lOperation in OperationList)
                {
                    if (lOperation.names.Equals(pOperationName))
                        tempResultOperation = lOperation;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in findOperationWithName, pOperationName: " + pOperationName);
                Console.WriteLine(ex.Message);
            }
            return tempResultOperation;
        }

        public List<string> getPreconditionForOperation(string opName)
        {
            List<string> con = null;
            try
            {
                operation op = findOperationWithName(opName);
                con = new List<string>(op.precondition);
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in getPreconditionForOperation");                
                Console.WriteLine(ex.Message);
            }
            return con;
        }

        public List<string> getPostconditionForOperation(string opName)
        {
            List<string> con = null;
            try
            {
                operation op = findOperationWithName(opName);
                con = new List<string>(op.postcondition);
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in getPostconditionForOperation");                
                Console.WriteLine(ex.Message);
            }
            return con;
        }

        public List<String> getActiveOperationNamesList(int pState)
        {
            List<String> tempActiveOperationNamesList = new List<string>();
            try
            {
                foreach (String operationName in ActiveOperationInstanceNamesList)
                {
                    if (getOperationStateFromOperationName(operationName).Equals(pState.ToString()))
                        tempActiveOperationNamesList.Add(operationName);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in getActiveOperationNamesList, pState: " + pState);
                Console.WriteLine(ex.Message);
            }
            return tempActiveOperationNamesList;
        }

        public String getOperationStateFromOperationName(String pOperationName)
        {
            String tempOperationState = "";
            try
            {
                String[] lOperationNameParts = pOperationName.Split('_');

                tempOperationState = lOperationNameParts[3];
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in getOperationStateFromOperationName, pOperationName: " + pOperationName);
                Console.WriteLine(ex.Message);
            }
            return tempOperationState;
        }

        public string getVariantGroup(string varName)
        {
            string lVariantgroup = "";
            try
            {
                variant var = findVariantWithName(varName);
                variantGroup varGroup = getVariantGroup(var);
                if (varGroup != null)
                    lVariantgroup = varGroup.names;
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in getVariantGroup");
                Console.WriteLine(ex.Message);
            }
            return lVariantgroup;
        }

        public List<variantGroup> getVariantGroupList()
        {
            return VariantGroupList;
        }

        public List<string> getConstraintList()
        {
            return ConstraintList;
        }

        public List<string> getvariantInstancesForOperation(string op)
        {
            List<string> instances = new List<string>();
            try
            {
                string[] opParts = new string[4];
                foreach (string iOp in ActiveOperationInstanceNamesList)
                {
                    opParts = iOp.Split('_');
                    if (String.Equals(opParts[0], op) && String.Equals(opParts[1], "F") && String.Equals(opParts[3], "0"))
                        instances.Add(opParts[2]);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in getvariantInstancesForOperation");                
                Console.WriteLine(ex.Message);
            }
            return instances;
        }

        public variantGroup getVariantGroup(variant var)
        {
            variantGroup lVariantGroup = null;
            try
            {
                foreach (variantGroup vg in VariantGroupList)
                {

                    foreach (variant v in vg.variant)
                    {
                        if (v.Equals(var))
                            lVariantGroup = vg;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in getVariantGroup");
                Console.WriteLine(ex.Message);
            }
            return lVariantGroup;
        }

        public void setActiveOperationInstanceNamesList(List<String> pActiveOperationInstanceNamesList)
        {
            ActiveOperationInstanceNamesList = pActiveOperationInstanceNamesList;
        }

        public void setInActiveOperationNamesList(List<String> pInActiveOperationNamesList)
        {
            InActiveOperationNamesList = pInActiveOperationNamesList;
        }

        public operation getOperationFromOperationName(string pOperationName)
        {
            operation resultOperation = null;
            try
            {
                string tempOperationName = "";
                if (pOperationName.Contains("_"))
                {
                    string[] pOperationNameParts = pOperationName.Split('_');
                    tempOperationName = pOperationNameParts[0];
                }
                else
                    tempOperationName = pOperationName;

                foreach (operation lOperation in OperationList)
                {
                    if (lOperation.names == tempOperationName)
                        resultOperation = lOperation;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in getOperationFromOperationName, pOperationName: " + pOperationName);
                Console.WriteLine(ex.Message);
            }
            return resultOperation;
        }

        /// <summary>
        /// This function checks the requirement field of the operations
        /// </summary>
        /// <returns>The result of the analysis of the requirment field of the operations</returns>
        public bool checkPreAnalysis()
        {
            bool lPreAnalysisResult = true;
            try
            {
                foreach (var operation in OperationList)
                {
                    if (!checkOperationRequirementField(operation))
                    {
                        Console.WriteLine(operation.names + " not executable!");
                        lPreAnalysisResult = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in checkPreAnalysis");
                Console.WriteLine(ex.Message);
            }
            return lPreAnalysisResult;
        }

        public bool checkOperationRequirementField(operation pOperation)
        {
            bool lCheckResult = false;
            try
            {
                //Check syntax of Requirement field
                lCheckResult = CheckOperationsRequirementFieldSyntax(pOperation.requirements);

                //for each part of (Trait)+ check that the traits are existing objects
                lCheckResult = CheckExistanceOfRequirementTraits(pOperation.requirements);

                //Check to find resource which inheritance field matches the  (Trait)+ part of the requirement field
                lCheckResult = CheckValidityOfOperationRequirementsTraits(pOperation.requirements);

                //For the fields in the expression of the requirement add the found resource name as a prefix to fields in expression
                AddRelevantResourceNameToOperationRequirementAttributes(pOperation);
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in checkOperationRequirementField, pOperationName: " + pOperation.names);
                Console.WriteLine(ex.Message);
            }
            return lCheckResult;
        }

        public string ReturnOperationRequirements(string pOperationName)
        {
            string lResultOperationRequirement = "";
            try
            {
                operation lResultingOperation = getOperationFromOperationName(pOperationName);

                foreach (string lRequirement in lResultingOperation.requirements)
                {
                    if (lResultOperationRequirement != "")
                        lResultOperationRequirement += " && " + lRequirement;
                    else
                        lResultOperationRequirement += lRequirement;
                }
                
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in ReturnOperationRequirements");
                Console.WriteLine(ex.Message);
            }
            return lResultOperationRequirement;
        }

        public List<resource> ReturnOperationChosenResource(string pOperationName)
        {
            List<resource> lResultResources = new List<resource>();
            try
            {
                //IMPORTANT: Here we have assumed that the operation requirement part is in the Prefix format
                //Also remember that the traits have been replaced
                //The operation has the format "operand operator1 resource_name.attribute"
                operation lOperation = findOperationWithName(pOperationName);

                foreach (string lRequirement in lOperation.requirements)
	            {
                    int lLastSpaceIndex = lRequirement.LastIndexOf(' ');
                    string lLastOperand = lRequirement.Substring(lLastSpaceIndex + 1);
                    string[] lLastOperandParts = lLastOperand.Split('_');
                    string lResourceName = lLastOperandParts[0];
                    lResultResources.Add(findResourceWithName(lResourceName));
	            }
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in ReturnOperationChosenResource");
                Console.WriteLine(ex.Message);
            }
            return lResultResources;
        }

        private void AddRelevantResourceNameToOperationRequirementAttributes(operation pOperation)
        {
            try
            {
                //IMPORTANT: We have the premise that the input is in the prefix format!!!
                List<string> lRequirementField = pOperation.requirements;

                List<Tuple<string, string>> lChangeRequirementList = new List<Tuple<string, string>>();

                foreach (string lRequirement in lRequirementField)
                {
                    //Here for each requirement we look at its traits and see which resource can match them
                    resource lResultingResource = ReturnRequirementMatchingResource(lRequirement);

                    //We find the attibute part of the requirement
                    string[] lRequirementFieldParts = lRequirement.Split(':');
                    string lAttributePart = lRequirementFieldParts[1].Trim();

                    //We add that resource name to add it to the field name of that requirement
                    int lIndexOfFirstOperand = lAttributePart.LastIndexOf(' ') + 1;
                    lAttributePart = lAttributePart.Insert(lIndexOfFirstOperand, lResultingResource.names + "_");
//                    lAttributePart = lResultingResource.names + "_" + lAttributePart;
//                    lAttributePart = lAttributePart.Replace(", ", ", " + lResultingResource.names + ".");

                    lChangeRequirementList.Add(new Tuple<string, string>(lRequirement, lAttributePart));
                }

                foreach (Tuple<string,string> lChangeRequirement in lChangeRequirementList)
                {
                    ChangeOperationRequirementField(pOperation, lChangeRequirement.Item1, lChangeRequirement.Item2);
                    
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in AddRelevantResourceNameToOperationRequirementAttributes");
                Console.WriteLine(ex.Message);
            }
        }

        private void ChangeOperationRequirementField(operation pOperation, string pOldRequirement, string pNewRequirement)
        {
            try
            {
                operation lOperation = findOperationWithName(pOperation.names);
                
                lOperation.requirements.Remove(pOldRequirement);

                lOperation.requirements.Add(pNewRequirement);

            }
            catch (Exception ex)
            {
                Console.WriteLine("error in ChangeOperationRequirementField");
                Console.WriteLine(ex.Message);
            }
        }

        private resource ReturnRequirementMatchingResource(string pRequirement)
        {
            resource lResultingResource = new resource();
            try
            {
                List<trait> lRequirementTraits = ExtractRequirementFieldTraits(pRequirement);

                List<resource> resources = (from resource in cResourceList
                                            where resource.traits.SequenceEqual(lRequirementTraits)
                                            select resource).ToList();

                if (resources.Count != 0)
                    lResultingResource = resources[0];
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in ReturnRequirementMatchingResource");
                Console.WriteLine(ex.Message);
            }
            return lResultingResource;
        }

        private bool CheckExistanceOfRequirementTraits(List<string> pRequirementField)
        {
            bool lSemanticCheck = true;
            try
            {
                foreach (var lRequirment in pRequirementField)
                {

                    string lTraitNamesStr = ExtractRequirementFieldTraitNames(lRequirment);

                    string[] lTraitNames = lTraitNamesStr.Split(',');

                    foreach (var lTraitName in lTraitNames)
                    {
                        var lTraits = from trait in cTraitList
                                      where trait.names == lTraitName.Trim()
                                      select trait;

                        if (lTraits != null)
                            lSemanticCheck = lSemanticCheck && true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in CheckExistanceOfRequirementTraits");
                Console.WriteLine(ex.Message);
            }
            return lSemanticCheck;
        }

        private bool CheckValidityOfOperationRequirementsTraits(List<string> pRequirementField)
        {
            bool lSemanticCheck = true;
            try
            {
                foreach (var lRequirment in pRequirementField)
                {
                    resource lResultingResource = ReturnRequirementMatchingResource(lRequirment);

                    if (lResultingResource != null)
                        lSemanticCheck = lSemanticCheck && true;
                }
            }
            catch (Exception ex)
            {
                lSemanticCheck = false;
                Console.WriteLine("error in CheckValidityOfOperationRequirementsTraits");
                Console.WriteLine(ex.Message);
            }
            return lSemanticCheck;
        }

        private string ExtractRequirementFieldTraitNames(string pRequirementField)
        {
            string requirementFieldTraitNames = "";
            try
            {
                string[] lRequirementFieldParts = pRequirementField.Split(':');
                requirementFieldTraitNames = lRequirementFieldParts[0].TrimEnd();
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in ExtractRequirementFieldTraitNames");
                Console.WriteLine(ex.Message);
            }
            return requirementFieldTraitNames;
        }

        private List<trait> ExtractRequirementFieldTraits(string pRequirementField)
        {
            List<trait> lRequirementFieldTraits = new List<trait>();
            string requirementFieldTraitNames = "";
            try
            {
                string[] lRequirementFieldParts = pRequirementField.Split(':');
                requirementFieldTraitNames = lRequirementFieldParts[0].TrimEnd();

                string[] lTraitNames = requirementFieldTraitNames.Split(',');
                for (int i = 0; i < lTraitNames.Length; i++)
			    {
                			 
                    foreach (var lTrait in TraitList)
                    {
                        if (lTrait.names == lTraitNames[i].Trim())
                        {
                            lRequirementFieldTraits.Add(lTrait);
                            break;
                        }
                    }
			    }
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in ExtractRequirementFieldTraits");
                Console.WriteLine(ex.Message);
            }
            return lRequirementFieldTraits;
        }

        private string ExtractResourceTraitNames(resource pResource)
        {
            string resourceTraitName = "";
            try
            {
                foreach (var trait in pResource.traits)
                {
                    if (resourceTraitName != "")
                        resourceTraitName += ",";
                    resourceTraitName += trait.names + " ";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in ExtractResourceTraitNames");
                Console.WriteLine(ex.Message);
            }
            return resourceTraitName;
        }

        private bool CheckOperationsRequirementFieldSyntax(List<string> pRequirementField)
        {
            bool lSyntaxCheck = true;
            try
            {
                foreach (var lRequirment in pRequirementField)
                {
                    
                    string lTraitPart = "";
                    string lExpressionPart = "";
                    //Requirement field grammar -> (Trait)+: expression
                    if (lRequirment.Contains(':'))
                    {
                        string[] lRequirementFieldParts = lRequirment.Split(':');
                        if (lRequirementFieldParts[0] != "" && lRequirementFieldParts[1] != "")
                        {
                            lTraitPart = lRequirementFieldParts[0].Trim();
                            lExpressionPart = lRequirementFieldParts[1].Trim();
                        }
                        else
                        {
                            lSyntaxCheck = false;
                            Console.WriteLine("error in CheckOperationsRequirementFieldSyntax, Operation requierment field should have the correct syntax");
                        }

                    }
                    else
                    {
                        lSyntaxCheck = false;
                        Console.WriteLine("error in CheckOperationsRequirementFieldSyntax, Operation requierment field should contain : character");
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("error in CheckOperationsRequirementFieldSyntax");
                Console.WriteLine(ex.Message);
            }
            return lSyntaxCheck;
        }

        /*
        public operation findOperationWithName(String pOperationName)
        {
            operation tempResultOperation = null;
            try
            {
                foreach (operation lOperation in OperationList)
                {
                    if (lOperation.names.Equals(pOperationName))
                        tempResultOperation = lOperation;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in findOperationWithName, pOperationName: " + pOperationName);
                Console.WriteLine(ex.Message);
            }
            return tempResultOperation;
        }
        */

        public List<variantOperations> getVariantsOperationsList()
        {
            return VariantsOperationsList;
        }

        public void setVariantsOperationsList(List<variantOperations> pVariantsOperationsList)
        {
            VariantsOperationsList = pVariantsOperationsList;
        }

        public string ReturnStringElements(List<String> pList)
        {
            string lResultElements = "";
            try
            {
                //TODO: write with LINQ
                foreach (string lElement in pList)
                {
                    lResultElements += lElement;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in ReturnStringElements");                
                Console.WriteLine(ex.Message);
            }
            return lResultElements;
        }


        public variant findVariantWithName(String pVariantName)
        {
            variant tempResultVariant = null;
            try
            {
                //TODO: write with LINQ
                foreach (variant lVariant in VariantList)
                {
                    if (lVariant.names.Equals(pVariantName))
                        tempResultVariant = lVariant;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in findVariantWithName, pVariantName: " + pVariantName);
                Console.WriteLine(ex.Message);
            }
            return tempResultVariant;
        }

        public variant findVariantWithIndex(int pVariantIndex)
        {
            variant lResultVariant = null;
            try
            {
                //TODO: write with LINQ
                foreach (variant lVariant in VariantList)
                {
                    if (lVariant.index.Equals(pVariantIndex))
                        lResultVariant = lVariant;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in findVariantWithIndex");
                Console.WriteLine(ex.Message);
            }
            return lResultVariant;
        }

        public int findLastVariantIndex()
        {
            int lVariantIndex = 0;
            try
            {
                //TODO: Write with LINQ
                foreach (variant lVariant in VariantList)
                {
                    if (lVariant.index > lVariantIndex)
                        lVariantIndex = lVariant.index;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in findLastVariantIndex");
                Console.WriteLine(ex.Message);
            }
            return lVariantIndex;
        }

        public void addVariantGroup(variantGroup pVariantGroup)
        {
            VariantGroupList.Add(pVariantGroup);
        }

        public void addVariant(variant pVariant)
        {
            VariantList.Add(pVariant);
        }

        public void addConstraint(String pConstraint)
        {
            ConstraintList.Add(pConstraint);
        }

        public void addOperation(operation pOperation)
        {
            OperationList.Add(pOperation);
        }

        public void addActiveOperationInstanceName(String pOperationInstanceName)
        {
            try
            {
                //TODO: for now just to be simple we will make the ActiveOperationNamesList just the names of the operations
                if (!ActiveOperationInstanceNamesList.Contains(pOperationInstanceName))
                    ActiveOperationInstanceNamesList.Add(pOperationInstanceName);
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in addActiveOperationInstanceName");
                Console.WriteLine(ex.Message);
            }
        }

        public void addActiveOperationName(String pOperationName)
        {
            try
            {
                //TODO: for now just to be simple we will make the ActiveOperationNamesList just the names of the operations
                if (!ActiveOperationNamesList.Contains(pOperationName))
                    ActiveOperationNamesList.Add(pOperationName);
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in addActiveOperationName");
                Console.WriteLine(ex.Message);
            }
        }

        public void addOperationInstance(string pOperationInstance)
        {
            try
            {
                if (!cOperationInstanceList.Contains(pOperationInstance))
                    cOperationInstanceList.Add(pOperationInstance);
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in addOperationInstance");
                Console.WriteLine(ex.Message);
            }
        }

        public void addStation(station pStation)
        {
            StationList.Add(pStation);
        }

        public station findStationWithName(String pStationName)
        {
            station tempResultStation = null;
            try
            {
                foreach (station lStation in StationList)
                {
                    if (lStation.names.Equals(pStationName))
                        tempResultStation = lStation;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in findStationWithName, pStationName: " + pStationName);
                Console.WriteLine(ex.Message);
            }
            return tempResultStation;
        }

        public void addResource(resource pResource)
        {
            ResourceList.Add(pResource);
        }

        public resource findResourceWithName(String pResourceName)
        {
            resource tempResultResource = null;
            try
            {
                foreach (resource lResource in ResourceList)
                {
                    if (lResource.names.Equals(pResourceName))
                        tempResultResource = lResource;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in findResourceWithName, pStationName: " + pResourceName);
                Console.WriteLine(ex.Message);
            }
            return tempResultResource;
        }

        public void addTrait(trait pTrait)
        {
            TraitList.Add(pTrait);
        }

        //TODO: Either this function or the next function is not needed
        /// <summary>
        /// Takes a specific operation and looks in the list of variant operations to see if this operation is part of any variant operation list
        /// </summary>
        /// <param name="pOperation">The operation we want to check if it is active</param>
        /// <returns>If an operation is active or not</returns>
        public bool isOperationActive(operation pOperation)
        {
            bool lOperationActive = false;
            try
            {
                foreach (variantOperations lVariantOperations in VariantsOperationsList)
                {
                    foreach (operation lOperation in lVariantOperations.getOperations())
                    {
                        if (lOperation.Equals(pOperation))
                            lOperationActive = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in isOperationActive");
                Console.WriteLine(ex.Message);
            }
            return lOperationActive;
        }

        //TODO: Either this function or the previous function is not needed
        /// <summary>
        /// In this function we have an operation name which we want to know if it is an active operation or not?
        /// </summary>
        /// <param name="pOperationName">Operation name which we want to check</param>
        /// <returns>If the operation is active or not</returns>
        public bool isActiveOperation(String pOperationName)
        {
            bool lResult = false;
            try
            {
                foreach (String lActiveOperationName in ActiveOperationInstanceNamesList)
                {
                    if (lActiveOperationName.Contains(pOperationName))
                    {
                        lResult = true;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in isActiveOperation, pOperationName: " + pOperationName);
                Console.WriteLine(ex.Message);
            }
            return lResult;
        }

        /// <summary>
        /// this function takes an operation instance variable name and looks at the variant code 
        /// if the variant code is 0 this means the operation instance is inactive hence the function will return false, otherwise it will return true
        /// </summary>
        /// <param name="pOperationInstanceVariableName">operation instance variable name</param>
        /// <returns>if the operation instance is active</returns>
        public bool isOperationInstanceActive(string pOperationInstanceVariableName)
        {
            bool lResult = false;

            try
            {
                string[] lOperationInstanceParts = pOperationInstanceVariableName.Split('_');
                if (!lOperationInstanceParts[2].Equals("0"))
                    lResult = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in isOperationInstanceActive");
                Console.WriteLine(ex.Message);
            }

            return lResult;
        }

        /// <summary>
        /// This function takes an instace variable of an operation and checks if it is in the initial state or not
        /// </summary>
        /// <param name="pOperationInstanceVariableName">Operaton instance variable name</param>
        /// <returns>If the operation instance variable is in the intial state or not</returns>
        public bool isOperationInstanceInitialState(string pOperationInstanceVariableName)
        {
            bool lResult = false;
            try
            {
                if (pOperationInstanceVariableName.Contains("_I_"))
                    lResult = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in isOperationInstanceInitialState");
                Console.WriteLine(ex.Message);
            }
            return lResult;
        }

        /// <summary>
        /// This function takes an instace variable of an operation and checks if it is in the unused state or not
        /// </summary>
        /// <param name="pOperationInstanceVariableName">Operaton instance variable name</param>
        /// <returns>If the operation instance variable is in the unused state or not</returns>
        public bool isOperationInstanceUnusedState(string pOperationInstanceVariableName)
        {
            bool lResult = false;
            try
            {
                if (pOperationInstanceVariableName.Contains("_U_"))
                    lResult = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in isOperationInstanceUnusedState");
                Console.WriteLine(ex.Message);
            }
            return lResult;
        }

        /// <summary>
        /// This funcion takes an operation instance and returns the operation name
        /// </summary>
        /// <param name="pOperationInstance"></param>
        /// <returns></returns>
        public string ReturnOperationNameFromOperationInstance(string pOperationInstance)
        {
            string lOperationName = "";
            try
            {
                string[] lOperationInstanceParts = pOperationInstance.Split('_');

                lOperationName = lOperationInstanceParts[0];
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in returnOperationNameFromOperationInstance");
                Console.WriteLine(ex.Message);
            }
            return lOperationName;
        }

        /// <summary>
        /// This funcion takes an operation instance and returns the operation
        /// </summary>
        /// <param name="pOperationInstance"></param>
        /// <returns></returns>
        public operation ReturnOperationFromOperationInstance(string pOperationInstance)
        {
            operation lResultOperation = null;
            try
            {
                string lOperationName = "";

                string[] lOperationInstanceParts = pOperationInstance.Split('_');

                lOperationName = lOperationInstanceParts[0];

                lResultOperation = findOperationWithName(lOperationName);
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in returnOperationNameFromOperationInstance");
                Console.WriteLine(ex.Message);
            }
            return lResultOperation;
        }

        /// <summary>
        /// This function takes an operation instance and returns the operation state from that operation instance
        /// </summary>
        /// <param name="pOperationInstance"></param>
        /// <returns></returns>
        public string ReturnOperationStateFromOperationInstance(string pOperationInstance)
        {
            string lOperationState = "";
            try
            {
                string[] lOperationInstanceParts = pOperationInstance.Split('_');
                lOperationState = lOperationInstanceParts[1];
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in ReturnOperationStateFromOperationInstance");
                Console.WriteLine(ex.Message);
                throw;
            }
            return lOperationState;
        }

        /// <summary>
        /// This function takes an operation instance and returns the operation variant from that operation instance
        /// </summary>
        /// <param name="pOperationInstance"></param>
        /// <returns></returns>
        public variant ReturnOperationVariantFromOperationInstance(string pOperationInstance)
        {
            variant lResultVariant;
            try
            {
                string lOperationVariantName = "";
                string[] lOperationInstanceParts = pOperationInstance.Split('_');
                lOperationVariantName = lOperationInstanceParts[2];
                lResultVariant = findVariantWithName(lOperationVariantName);
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in ReturnOperationVariantFromOperationInstance");
                Console.WriteLine(ex.Message);
                throw;
            }
            return lResultVariant;
        }

        /// <summary>
        /// This function takes an operation instance and returns the operation transition from that operation instance
        /// </summary>
        /// <param name="pOperationInstance"></param>
        /// <returns></returns>
        public string ReturnOperationTransitionFromOperationInstance(string pOperationInstance)
        {
            string lOperationState = "";
            try
            {
                string[] lOperationInstanceParts = pOperationInstance.Split('_');
                lOperationState = lOperationInstanceParts[3];
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in ReturnOperationTransitionFromOperationInstance");
                Console.WriteLine(ex.Message);
                throw;
            }
            return lOperationState;
        }

        public void addInActiveOperationName(String pOperationName)
        {
            try
            {
                //TODO: for now just to be simple we will make the ActiveOperationNamesList just the names of the operations
                if (!InActiveOperationNamesList.Contains(pOperationName))
                    InActiveOperationNamesList.Add(pOperationName);
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in addInActiveOperationName");
                Console.WriteLine(ex.Message);
            }
        }

        public String giveNextStateActiveOperationName(String pActiveOperationName)
        {
            String lNextStateActiveOperationName = "";
            try
            {
                String[] parts = pActiveOperationName.Split('_');
                if (parts[3] != null)
                {
                    int lCurrentActiveOperationIndex = Convert.ToInt32(parts[3]);
                    parts[3] = (lCurrentActiveOperationIndex + 1).ToString();
                }

                lNextStateActiveOperationName = String.Join("_", parts);
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in giveNextStateActiveOperationName, pActiveOperationName: " + pActiveOperationName);                
                Console.WriteLine(ex.Message);
            }
            return lNextStateActiveOperationName;
        }

        public string getOperationNameFromActiveOperation(string pActiveOperationName)
        {
            string lResultOperationName = "";
            try
            {
                string[] parts = pActiveOperationName.Split('_');
                //ActiveOperationInstance: OperationName_State_Variant_Transition
                if (parts.Length >= 1)
                    lResultOperationName = parts[0];
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in getOperationNameFromActiveOperation");
                Console.WriteLine(ex.Message);
            }
            return lResultOperationName;
        }

        public operation getOperationFromActiveOperation(string pActiveOperationName)
        {
            operation lResultOperation = null;
            try
            {
                string[] parts = pActiveOperationName.Split('_');
                //ActiveOperationInstance: OperationName_State_Variant_Transition
                if (parts.Length >= 1)
                    lResultOperation = findOperationWithName(parts[0]);
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in getOperationFromActiveOperation");
                Console.WriteLine(ex.Message);
            }
            return lResultOperation;
        }

        public variant getVariantFromActiveOperationName(string pActiveOperationName)
        {
            variant lResultVariant = null;
            try
            {
                string[] parts = pActiveOperationName.Split('_');
                //ActiveOperationInstance: OperationName_State_Variant_Transition
                if (parts.Length >= 2)
                    lResultVariant = findVariantWithIndex(int.Parse(parts[2]));
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in getVariantFromActiveOperation");
                Console.WriteLine(ex.Message);
            }
            return lResultVariant;
        }

        public int getVariantIndexFromActiveOperation(String pActiveOperationName)
        {
            int lVariantIndex = -1;
            try
            {
                String[] parts = pActiveOperationName.Split('_');
                //ActiveOperationInstance: OperationName_State_Variant_Transition
                if (parts.Length >= 3)
                {
                    if (parts[2] != null)
                        lVariantIndex = Convert.ToInt32(parts[2]);
                }
                else
                    //This means that the variant for the active operation has not been mentioned so this operation should be considered for all active variants
                    lVariantIndex = -1;
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in getVariantIndexFromActiveOperation, pActiveOperationName: " + pActiveOperationName);
                Console.WriteLine(ex.Message);
            }

            return lVariantIndex;
        }

        public int getOperationTransitionNumberFromActiveOperation(string pActiveOperationName)
        {
            int lOpTransNum = 0;
            try
            {
                if (!pActiveOperationName.Contains("Possible") && !pActiveOperationName.Contains("Use"))
                {
                    String[] parts = pActiveOperationName.Split('_');
                    //ActiveOperationInstance: OperationName_State_Variant_Transition
                    if (parts.Length.Equals(4))
                    {
                        if (parts[3] != null)
                            lOpTransNum = Convert.ToInt32(parts[3]);
                    }
                    else
                    {
                        //This means that for the active operation the transition number has not been mentioned hence it should be considered from the first transition
                        lOpTransNum = calculateTransitionNumberForActiveOperation(pActiveOperationName);
                        setActiveOperationMissingTransitionNumber(pActiveOperationName, lOpTransNum);
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("error in getOperationTransitionNumberFromActiveOperation, pActiveOperationName: " + pActiveOperationName);
                Console.WriteLine(ex.Message);
            }
            return lOpTransNum;
        }

        private void calculateVariantIndexForActiveOperation(string pActiveOperationName)
        {
            try
            {
                variant lActiveVariant = getVariantFromActiveOperationName(pActiveOperationName);
                operation lActiveOperation = getOperationFromActiveOperation(pActiveOperationName);
                //Here we have to find any variantOperations which ourActive Operation is part of
                foreach (variantOperations lVariantOperations in VariantsOperationsList)
                {

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in calculateVariantIndexForActiveOperation");
                Console.WriteLine(ex.Message);
            }
        }

        private int calculateTransitionNumberForActiveOperation(string pActiveOperationName)
        {
            int lTransitionNo = 0;
            try
            {
                variant lActiveVariant = getVariantFromActiveOperationName(pActiveOperationName);
                operation lActiveOperation = getOperationFromActiveOperation(pActiveOperationName);
                //Here we have to find any variantOperations which ourActive Operation is part of
                foreach (variantOperations lVariantOperations in VariantsOperationsList)
                {
                    string lVariantExpression = lVariantOperations.getVariantExpr();

                    if (lVariantExpression.Contains(lActiveVariant.names))
                    {
                        lTransitionNo = 0;
                        foreach (operation lOperation in lVariantOperations.getOperations())
                        {
                            lTransitionNo += 2;
                            if (lOperation.Equals(lActiveOperation))
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in calculateTransitionNumberForActiveOperation");
                Console.WriteLine(ex.Message);
            }
            return lTransitionNo;
        }

        private void setActiveOperationMissingTransitionNumber(string pActiveOperationName, int pCalculatedTransitionNumber)
        {
            try
            {
                string lNewOperationName = pActiveOperationName + "_" + pCalculatedTransitionNumber;

                //Here we know that the operation does not have a transition number hence we set the transition number of the operation to 0
                updateOperationName(pActiveOperationName, lNewOperationName);

                refactorOperationName(pActiveOperationName, lNewOperationName);
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in setActiveOperationMissingTransitionNumber");
                Console.WriteLine(ex.Message);
            }
        }

        private void refactorOperationName(string pOldOperationName, string pNewOperationName)
        {
            try
            {
                //But as this operation name has changed we have to change any reference which is to this operation:
                //1. First in the local operation list
                updateOperationNameInLocalList(pOldOperationName, pNewOperationName);

                //2. Second in the pre-condition or post condition of any of the other operations
                updateOperationNameInPrePostConditions(pOldOperationName, pNewOperationName);

                //3. Third in the variant-operation mappings
                updateOperationNameInVariantOperationMapping(pOldOperationName, pNewOperationName);

            }
            catch (Exception ex)
            {
                Console.WriteLine("error in refactorOperationName");
                Console.WriteLine(ex.Message);
            }
        }

        private void updateOperationName(string pOldOperationName, string pNewOperationName)
        {
            try
            {
                operation lFoundOperation = OperationList.Find(item => item.names.Equals(pOldOperationName));
                if (lFoundOperation != null)
                    lFoundOperation.names = pNewOperationName;
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in updateOperationName");
                Console.WriteLine(ex.Message);
            }
        }

        private void updateOperationNameInVariantOperationMapping(string pOldOperationName, string pNewOperationName)
        {
            try
            {
                //In this function the name of one of the operations in the local list has changed so we want to update any possible references to this operation in the variantOperationsList
                foreach (variantOperations lVariantOperations in VariantsOperationsList)
                {
                    var lOperation = lVariantOperations.getOperations().Find(item => item.names.Contains(pOldOperationName));
                    if (lOperation != null)
                    {
                        lVariantOperations.getOperations().Remove(lOperation);
                        lOperation.names = pNewOperationName;
                        lVariantOperations.getOperations().Add(lOperation);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in updateOperationNameInVariantOperationMapping");
                Console.WriteLine(ex.Message);
            }
        }

        private void updateOperationNameInPrePostConditions(string pOldOperationName, string pNewOperationName)
        {
            try
            {
                
                //In this function the name of one of the operations in the local list has changed so we want to update the pre/post condition of any operation that references this operation
                foreach (operation lOperation in cOperationList)
                {
                    var lPrecondition = lOperation.precondition.Find(item => item.Contains(pOldOperationName));
                    if (lPrecondition != null)
                    {
                        lOperation.precondition.Remove(pOldOperationName);
                        lOperation.precondition.Add(pNewOperationName);
                    }

                    var lPostCondition = lOperation.postcondition.Find(item => item.Contains(pOldOperationName));
                    if (lPostCondition != null)
                    {
                        lOperation.postcondition.Remove(pOldOperationName);
                        lOperation.postcondition.Add(pNewOperationName);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in updateOperationNameInPrePostConditions");
                Console.WriteLine(ex.Message);
            }
        }

        private void updateOperationNameInLocalList(string pOldOperationName, string pNewOperationName)
        {
            try
            {
                //In this function the name of one of the operations in the local list has changed so we want to update the local operation list
                operation lOperationToChangeName = cOperationList.Find(item => item.names.Equals(pOldOperationName));
                if (lOperationToChangeName != null)
                    lOperationToChangeName.names = pNewOperationName;

            }
            catch (Exception ex)
            {
                Console.WriteLine("error in updateOperationNameInLocalList");
                Console.WriteLine(ex.Message);
            }
        }

        public void addVariantsOperations(variantOperations pVariantOperations)
        {
            VariantsOperationsList.Add(pVariantOperations);
        }

        public void PrintDataSummary()
        {
            try
            {
                //In this function we will print a summary of the important information which is contained in the framework wrapper
                string lDataSummary = "";

                //Operations
                if (cOperationList.Count > 0)
                {
                    lDataSummary += "Operations:" + System.Environment.NewLine;
                    foreach (operation lOperation in OperationList)
                    {
                        lDataSummary += "Operation Name: " + lOperation.names + System.Environment.NewLine;
                        lDataSummary += "Operation Display Name: " + lOperation.displayName + System.Environment.NewLine;
                        foreach (string lPreconditionOperationName in lOperation.precondition)
                            lDataSummary += "Operation Precondition: " + lPreconditionOperationName + System.Environment.NewLine;

                        foreach (string lPostconditionOperationName in lOperation.postcondition)
                            lDataSummary += "Operation Postcondition: " + lPostconditionOperationName + System.Environment.NewLine;
                    }
                }

                //Variants
                if (cVariantList.Count > 0)
                {
                    lDataSummary += "Variants:" + System.Environment.NewLine;
                    foreach (variant lVariant in VariantList)
                    {
                        lDataSummary += "Variant Name: " + lVariant.names + System.Environment.NewLine;
                        lDataSummary += "Variant Index: " + lVariant.index + System.Environment.NewLine;
                        lDataSummary += "Variant Display Name: " + lVariant.displayName + System.Environment.NewLine;
                        /*                    foreach (operation lManOperation in lVariant.manOperations)
                                                lDataSummary += "Variant Manufacturing Operation: " + lManOperation.names + System.Environment.NewLine;*/
                    }

                    //Variantgroups
                    lDataSummary += "Variant Groups:" + System.Environment.NewLine;
                    foreach (variantGroup lVariantGroup in VariantGroupList)
                    {
                        lDataSummary += "Variant Group Name: " + lVariantGroup.names + System.Environment.NewLine;
                        lDataSummary += "Variant Group Cardinality: " + lVariantGroup.gCardinality + System.Environment.NewLine;
                        foreach (variant lVariant in lVariantGroup.variant)
                            lDataSummary += "Variant Group Variant: " + lVariant.displayName + System.Environment.NewLine;

                    }
                }

                if (VariantsOperationsList.Count > 0)
                {
                    //VariantOperationMappings
                    lDataSummary += "Variant Operation Mappings:" + System.Environment.NewLine;
                    foreach (variantOperations lVariantOperations in VariantsOperationsList)
                    {
                        lDataSummary += "Variant Name: " + lVariantOperations.getVariantExpr() + System.Environment.NewLine;
                        lDataSummary += "Operations: " + System.Environment.NewLine;
                        foreach (operation lOperation in lVariantOperations.getOperations())
                        {
                            lDataSummary += "Operation Name: " + lOperation.names + System.Environment.NewLine;
                        }
                    }
                }

                //Traits
                if (cTraitList.Count > 0)
                {
                    lDataSummary += "Traits:" + System.Environment.NewLine;
                    foreach (trait lTrait in cTraitList)
                    {
                        lDataSummary += "Trait Name: " + lTrait.names + System.Environment.NewLine;
                        lDataSummary += "Attributes: " + System.Environment.NewLine;
                        foreach (Tuple<string, string> lAttribute in lTrait.attributes)
                        {
                            lDataSummary += "Attribute Name: " + lAttribute.Item2 + System.Environment.NewLine;
                            lDataSummary += "Attribute Type: " + lAttribute.Item1 + System.Environment.NewLine;
                        }
                    }
                }

                System.Console.WriteLine(lDataSummary);
                System.IO.File.WriteAllText("D:/LocalImplementation/GitHub/ProductPlatformAnalyzer/ProductPlatformAnalyzer/Output/Debug/DataSummary.txt", lDataSummary);

                //System.IO.File.WriteAllText("C:/Users/amir/Desktop/Output/Debug/DataSummary.txt", lDataSummary);

            }
            catch (Exception ex)
            {
                Console.WriteLine("error in PrintDataSummary");
                Console.WriteLine(ex.Message);
            }
        }

        public void CreateOperationInstance(string pName
                                            , string pDisplayName
                                            , List<string> pRequirements
                                            , List<string> pPreconditions
                                            , List<string> pPostconditions)
        {
            try
            {
                operation tempOperation = new operation();
                tempOperation.names = pName;
                tempOperation.displayName = pDisplayName;
                tempOperation.requirements = pRequirements;
                if (pPreconditions != null)
                {
                    List<String> tempPrecondition = new List<String>();
                    foreach (String lOperationName in pPreconditions)
                    {
                        tempPrecondition.Add(lOperationName);
                    }
                    tempOperation.precondition = tempPrecondition;
                }
                if (pPostconditions != null)
                {
                    List<String> tempPostcondition = new List<String>();
                    foreach (String lOperationName in pPostconditions)
                    {
                        tempPostcondition.Add(lOperationName);
                    }
                    tempOperation.postcondition = tempPostcondition;
                }
                addOperation(tempOperation);
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in CreateOperationInstance, pName: " + pName 
                                                                + " ,pDisplayName: " + pDisplayName 
                                                                + " ,pPreconditions: " + ReturnStringElements(pPreconditions) 
                                                                + " ,pPostconditions: " + ReturnStringElements(pPostconditions));
                Console.WriteLine(ex.Message);
            }
        }

        public void CreateVariantInstance(string pName, int pIndex, string pDisplayName)
        {
            try
            {
                variant tempVariant = new variant();
                tempVariant.names = pName;
                tempVariant.index = pIndex;
                tempVariant.displayName = pDisplayName;
                addVariant(tempVariant);
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in CreateVariantInstance, pName: " + pName
                                                                + " ,pIndex: " + pIndex.ToString()
                                                                + " ,pDisplayName: " + pDisplayName);
                Console.WriteLine(ex.Message);
            }
        }

        public void CreateVariantGroupInstance(string pName, string pGroupCardinality, List<string> pVariantList)
        {
            try
            {
                variantGroup tempVariantGroup = new variantGroup();
                tempVariantGroup.names = pName;
                tempVariantGroup.gCardinality = pGroupCardinality;
                if (pVariantList != null)
                {
                    List<variant> tempVariantList = new List<variant>();
                    foreach (string lVariantName in pVariantList)
                    {
                        tempVariantList.Add(findVariantWithName(lVariantName));
                    }
                    tempVariantGroup.variant = tempVariantList;
                }
                addVariantGroup(tempVariantGroup);
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in CreateVariantGroupInstance, pName: " + pName
                                                                + " ,pGroupCardinality: " + pGroupCardinality
                                                                + " ,pVariantList: " + ReturnStringElements(pVariantList));
                Console.WriteLine(ex.Message);
            }
        }

        public variantOperations CreateVariantOperationMappingTemporaryInstance(string pVariantName, List<string> pOperationList)
        {
            variantOperations lVariantOperations = new variantOperations();
            try
            {
                //Here we have to check if the variant name is a single variant or a variant expression


                //lVariantOperations.setVariantExpr(findVariantWithName(pVariantName));
                lVariantOperations.setVariantExpr(pVariantName);

                if (pOperationList != null)
                {
                    List<operation> tempOperations = new List<operation>();
                    foreach (string lOperationName in pOperationList)
                    {
                        tempOperations.Add(getOperationFromOperationName(lOperationName));
                    }
                    lVariantOperations.setOperations(tempOperations);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("error in CreateVariantOperationMappingInstance, pVariantName: " + pVariantName
                                                                + " ,pOperationList: " + ReturnStringElements(pOperationList));
                Console.WriteLine(ex.Message);
            }
            return lVariantOperations;
        }

        public void CreateVariantOperationMappingInstance(string pVariantName, List<string> pOperationList)
        {
            try
            {
                //Here we have to check if the variant name is a single variant or a variant expression

                variantOperations lVariantOperations = new variantOperations();

                //lVariantOperations.setVariantExpr(findVariantWithName(pVariantName));
                lVariantOperations.setVariantExpr(pVariantName);

                if (pOperationList != null)
                {
                    List<operation> tempOperations = new List<operation>();
                    foreach (String lOperationName in pOperationList)
                    {
                        tempOperations.Add(getOperationFromOperationName(lOperationName));
                    }
                    lVariantOperations.setOperations(tempOperations);
                }
                addVariantsOperations(lVariantOperations);
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in CreateVariantOperationMappingInstance, pVariantName: " + pVariantName
                                                                + " ,pOperationList: " + ReturnStringElements(pOperationList));
                Console.WriteLine(ex.Message);
            }
        }

        public void CreateVariantOperationMappingInstance(string pVariantName, List<operation> pOperationList)
        {
            try
            {
                //Here we have to check if the variant name is a single variant or a variant expression

                variantOperations lVariantOperations = new variantOperations();

                //lVariantOperations.setVariantExpr(findVariantWithName(pVariantName));
                lVariantOperations.setVariantExpr(pVariantName);

                lVariantOperations.setOperations(pOperationList);

                addVariantsOperations(lVariantOperations);
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in CreateVariantOperationMappingInstance");
                Console.WriteLine(ex.Message);
            }
        }

        private trait findTraitWithName(string pTraitName)
        {
            trait lResultTrait = new trait();
            try
            {
                List<trait> lTempResultTrait = (from trait in cTraitList
                                        where trait.names == pTraitName
                                        select trait).ToList();
                if (lTempResultTrait.Count == 1)
                    lResultTrait = lTempResultTrait[0];
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in findTraitWithName");
                Console.WriteLine(ex.Message);
            }
            return lResultTrait;
        }

        private void CreateStationInstance(string pName, List<string> pStationResources)
        {
            try
            {
                station tempStation = new station();
                tempStation.names = pName;
                if (pStationResources != null)
                {
                    List<resource> tempResourceList = new List<resource>();
                    foreach (String lResourceName in pStationResources)
                    {
                        tempResourceList.Add(findResourceWithName(lResourceName));
                    }
                    tempStation.resources = tempResourceList;
                }
                addStation(tempStation);
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in CreateStationInstance, pName: " + pName
                                                                + " ,pResourceList: " + ReturnStringElements(pStationResources));
                Console.WriteLine(ex.Message);
            }
        }

        public void createTraitInstance(string pName, List<trait> pInherit, List<Tuple<string,string>> pAttributes)
        {
            try
            {
                trait tempTrait = new trait();

                tempTrait.names = pName;
                tempTrait.inherit = pInherit;
                tempTrait.attributes = pAttributes;

                addTrait(tempTrait);
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in createTraitInstance, pName: " + pName);
                Console.WriteLine(ex.Message);
            }
        }

        public void CreateResourceInstance(string pName, List<trait> pTraits ,List<Tuple<string,string,string>> pAttributes)
        {
            try
            {
                resource tempResource = new resource();
                tempResource.names = pName;
                tempResource.traits = pTraits;
                tempResource.attributes = pAttributes;
                addResource(tempResource);
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in CreateResourceInstance, pName: " + pName);
                Console.WriteLine(ex.Message);
            }
        }

        public variant createVirtualVariant(string lVariantExpr)
        {
            variant lResultVariant = new variant();
            try
            {
                //A new Virtual variant need to be build
                //AND
                //The virtual variant needs to be added to virtual variant group
                variant lVirtualVariant = createVirtualVariantInstance();

                addVirtualVariantToGroup(lVirtualVariant);

                //A new constraint needs to be added relating the virtual variant to the variant expression
                addVirtualVariantConstaint(lVirtualVariant, lVariantExpr);

                //The new virtual variant and the exression it represents needs to be added to virtual variant list in frameworkwrapper
                createVirtualVariant2VariantExprInstance(lVirtualVariant, lVariantExpr);

                lResultVariant = lVirtualVariant;
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in createVirtualVariant");
                Console.WriteLine(ex.Message);
            }
            return lResultVariant;
        }

        //TODO: Do we need this anymore?
        public variant ReturnCurrentVariant(variantOperations pVariantOperations)
        {
            variant lResultVariant = null;
            try
            {

                string lVariantExpr = pVariantOperations.getVariantExpr();

                if (lVariantExpr.Contains(' '))
                {
                    //Then this should be an expression on variants
                    lResultVariant = createVirtualVariant(lVariantExpr);
                }
                else
                {
                    //Then this should be a single variant
                    lResultVariant = findVariantWithName(lVariantExpr);
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine("error in ReturnCurrentVariant");
                Console.WriteLine(ex.Message);
            }
            return lResultVariant;
        }


        public void addVirtualVariantConstaint(variant pVirtualVariant, string pVariantExpr)
        {
            try
            {
                //A new constraint needs to be added relating the virtual variant to the variant expression
                addConstraint("-> " + pVirtualVariant.names + " (" + pVariantExpr + ")");
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in addVirtualVariantConstaint");
                Console.WriteLine(ex.Message);
            }
        }

        public void createVirtualVariant2VariantExprInstance(variant pVirtualVariant, string pVariantExpr)
        {
            try
            {
                //The new virtual variant and the exression it represents needs to be added to virtual variant list in frameworkwrapper
                virtualVariant2VariantExpr lTempVirtualVariant = new virtualVariant2VariantExpr();

                lTempVirtualVariant.setVirtualVariant(pVirtualVariant);
                lTempVirtualVariant.setVariantExpr(pVariantExpr);

                cVirtualVariant2VariantExprList.Add(lTempVirtualVariant);

            }
            catch (Exception ex)
            {
                Console.WriteLine("error in createVirtualVariant2VariantExprInstance");
                Console.WriteLine(ex.Message);
            }
        }

        public variant createVirtualVariantInstance()
        {
            variant lVirtualVariant = new variant();
            try
            {
                string name = "VirtualV" + getNextVirtualIndex();
                int variantIndex = getNextVariantIndex();
                CreateVariantInstance(name, variantIndex, name);

                lVirtualVariant = findVariantWithName(name);

            }
            catch (Exception ex)
            {
                Console.WriteLine("error in createVirtualVariantInstance");
                Console.WriteLine(ex.Message);
            }


            return lVirtualVariant;
        }

        public void addVirtualVariantToGroup(variant var)
        {
            try
            {
                foreach (variantGroup vg in VariantGroupList)
                {
                    if (string.Equals(vg.names, "Virtual-VG"))
                    {
                        vg.variant.Add(var);
                        return;
                    }
                }

                List<string> varName = new List<string>();
                varName.Add(var.names);

                CreateVariantGroupInstance("Virtual-VG", "choose any number", varName);
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in addVirtualVariantToGroup");
                Console.WriteLine(ex.Message);
            }
        }


        /*private void addVirtualConstraint(virtualConnection connection)
        {
            string constraint = "or";
            List<variant> variants = new List<variant>(connection.getVariants());
            string andVariants = variants.ElementAt(0).names;
            variants.RemoveAt(0);

            foreach (variant var in variants)
            {
                andVariants = "(and " + andVariants + " " + var.names + ")";
            }

            constraint = constraint + " (and " + andVariants + " " + connection.getVirtualVariant().names + ") (and (not " +
                            andVariants + ") (not " + connection.getVirtualVariant().names + "))";

            ConstraintList.Add(constraint);
        }*/

        /*internal virtualConnection findVirtualConnectionWithName(string p)
        {
            foreach (virtualConnection con in VirtualConnections)
            {
                if (String.Equals(con.getVirtualVariant().names, p))
                    return con;
            }

            return null;
        }*/

        private int getNextVariantIndex()
        {
            int index = 0;
            try
            {
                foreach (variant var in VariantList)
                {
                    if (var.index > index)
                        index = var.index;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in getNextVariantIndex");
                Console.WriteLine(ex.Message);
            }
            return index + 1;
        }

        private int getVirtualVariantIndex(variant pVirtualVariant)
        {
            int lVirtualVariantIndex = 0;
            try
            {
                string lVirtualVariantName = pVirtualVariant.names;
                int lStrartingIndex = lVirtualVariantName.IndexOf("VirtualVariant");
                lVirtualVariantIndex = int.Parse(lVirtualVariantName.Remove(lStrartingIndex + 14 + 1));
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in getVirtualVariantIndex");
                Console.WriteLine(ex.Message);
            }
            return lVirtualVariantIndex;
        }

        private int getMaxVirtualVariantNumber()
        {
            int lResultIndex = 0; 
            try
            {
                foreach (virtualVariant2VariantExpr virtualVariant2VariantExpr in cVirtualVariant2VariantExprList)
                {
                    variant virtualVariant = virtualVariant2VariantExpr.getVirtualVariant();

                    lResultIndex = getVirtualVariantIndex(virtualVariant) + 1;

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in getMaxVirtualVariantNumber");
                Console.WriteLine(ex.Message);
            }
            return lResultIndex;
        }

        private string getNextVirtualVariantName()
        {
            string lResultName = "";
            try
            {
                //Each virtual variant is named as "VirtualVariantX" which X is a number
                int lMaxVirtualVariantNumber = getMaxVirtualVariantNumber();

                lResultName = "VirtualVariant" + lMaxVirtualVariantNumber;
                
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in getNextVirtualVariantName");
                Console.WriteLine(ex.Message);
            }
            return lResultName;
        }

        public string findVirtualVariantExpression(string lVirtualVariant)
        {
            string lVirtualVariantExpr = "";
            try
            {
                //TODO: if possible rewrite with LINQ
                foreach (virtualVariant2VariantExpr virtualVariant in cVirtualVariant2VariantExprList)
                {
                    if (virtualVariant.getVirtualVariant().names == lVirtualVariant)
                        lVirtualVariantExpr = virtualVariant.getVariantExpr();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in findVirtualVariantExpression");                
                Console.WriteLine(ex.Message);
            }
            return lVirtualVariantExpr;
        }


        public String getXMLNodeAttributeInnerText(XmlNode lNode, String lAttributeName)
        {
            String lResultAttributeText = "";

            try
            {
                lResultAttributeText =lNode[lAttributeName].InnerText;
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in getXMLNodeAttributeInnerText, node " + lNode.Name + " does not have attribute " + lAttributeName);
                Console.WriteLine(ex.Message);
            }
            return lResultAttributeText;
        }

        //TODO: rename this function to show that you are loading from input
        public List<variantOperations> createVariantOperationTemporaryInstances(XmlDocument pXDoc)
        {
            List<variantOperations> lVariantOperationsList = new List<variantOperations>();
            try
            {
                XmlNodeList nodeList = pXDoc.DocumentElement.SelectNodes("//variantOperationMapping");


                foreach (XmlNode lNode in nodeList)
                {
                    List<string> lVariantOperations = new List<string>();

                    XmlNodeList variantOperationsNodeList = lNode["operationRefs"].ChildNodes;
                    foreach (XmlNode lVariantOperation in variantOperationsNodeList)
                    {
                        lVariantOperations.Add(lVariantOperation.InnerText);
                    }


                    lVariantOperationsList.Add(CreateVariantOperationMappingTemporaryInstance(getXMLNodeAttributeInnerText(lNode, "variantRefs")
                                                                    , lVariantOperations));


                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in createVariantOperationInstances");
                Console.WriteLine(ex.Message);
            }
            return lVariantOperationsList;
        }

        //TODO: rename this function to show that you are loading from input
        public bool createTraitInstances(XmlDocument pXDoc)
        {
            bool lDataLoaded = false;
            try
            {
                XmlNodeList nodeList = pXDoc.DocumentElement.SelectNodes("//trait");

                if (nodeList.Count.Equals(0))
                    lDataLoaded = false;
                else
                {
                    foreach (XmlNode lNode in nodeList)
                    {
                        List<Tuple<string, string>> lAttributes = new List<Tuple<string, string>>();

                        List<trait> lInheritTraits = new List<trait>();

                        XmlNodeList inheritList = lNode["inherit"].ChildNodes;
                        foreach (XmlNode lTraitName in inheritList)
                        {
                            lInheritTraits.Add(findTraitWithName(lTraitName.InnerText));
                        }

                        XmlNodeList attributeList = lNode["attributes"].ChildNodes;
                        foreach (XmlNode lAttribute in attributeList)
                        {
                            lAttributes.Add(new Tuple<string, string>(getXMLNodeAttributeInnerText(lAttribute, "attributeType")
                                            , getXMLNodeAttributeInnerText(lAttribute, "attributeName")));
                        }

                        createTraitInstance(getXMLNodeAttributeInnerText(lNode, "traitName")
                                                                , lInheritTraits
                                                                , lAttributes);
                    }
                    lDataLoaded = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in createTraitInstances");
                Console.WriteLine(ex.Message);
            }
            return lDataLoaded;
        }

        //TODO: rename this function to show that you are loading from input
        public bool createResourceInstances(XmlDocument pXDoc)
        {
            bool lDataLoaded = false;
            try
            {
                XmlNodeList nodeList = pXDoc.DocumentElement.SelectNodes("//resource");

                if (nodeList.Count.Equals(0))
                    lDataLoaded = false;
                else
                {
                    foreach (XmlNode lNode in nodeList)
                    {
                        List<Tuple<string, string, string>> lAttributes = new List<Tuple<string, string, string>>();
                        List<trait> lTraits = new List<trait>();
                        XmlNodeList traitNamesList = lNode["traits"].ChildNodes;
                        foreach (XmlNode lTraitRef in traitNamesList)
                        {
                            string lTraitName = lTraitRef.InnerText;
                            if (lTraitName != "")
                                lTraits.Add(findTraitWithName(lTraitName));
                        }

                        XmlNodeList attributeList = lNode["attributes"].ChildNodes;
                        foreach (XmlNode lAttribute in attributeList)
                        {
                            lAttributes.Add(new Tuple<string, string, string>(getXMLNodeAttributeInnerText(lAttribute, "attributeName")
                                                                            , getXMLNodeAttributeInnerText(lAttribute, "attributeType")
                                                                            , getXMLNodeAttributeInnerText(lAttribute, "attributeValue")));
                        }


                        CreateResourceInstance(getXMLNodeAttributeInnerText(lNode, "resourceName")
                                                                , lTraits
                                                                , lAttributes);
                    }
                    lDataLoaded = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in createResourceInstances");
                Console.WriteLine(ex.Message);
            }
            return lDataLoaded;
        }

        //TODO: rename this function to show that you are loading from input
        private void createStationInstances(XmlDocument pXDoc)
        {
            try
            {
                XmlNodeList nodeList = pXDoc.DocumentElement.SelectNodes("//station");

                foreach (XmlNode lNode in nodeList)
                {
                    List<string> lStationResources = new List<string>();

                    XmlNodeList stationResourcesNodeList = lNode["resourceRefs"].ChildNodes;
                    foreach (XmlNode lStationResource in stationResourcesNodeList)
                    {
                        lStationResources.Add(lStationResource.InnerText);
                    }


                    CreateStationInstance(getXMLNodeAttributeInnerText(lNode, "stationName")
                                                            , lStationResources);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in createStationInstances");
                Console.WriteLine(ex.Message);
            }
        }

        //TODO: rename this function to show that you are loading from input
        public bool createConstraintInstances(XmlDocument pXDoc)
        {
            bool lDataLoaded = false;
            try
            {
                XmlNodeList nodeList = pXDoc.DocumentElement.SelectNodes("//constraint");

                if (nodeList.Count.Equals(0))
                    lDataLoaded = false;
                else
                {
                    foreach (XmlNode lNode in nodeList)
                    {
                        string lPrefixFormat = getXMLNodeAttributeInnerText(lNode, "logic");
                        //TODO: if the constraint are converted into infix format this line has to be used
                        //string lPrefixFormat = GeneralUtilities.parseExpression(lPrefixFormat, "prefix");
                        addConstraint(lPrefixFormat);
                    }
                    lDataLoaded = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in createConstraintInstances");                
                Console.WriteLine(ex.Message);
            }
            return lDataLoaded;
        }

        //TODO: rename this function to show that you are loading from input
        public bool createVariantGroupInstances(XmlDocument pXDoc)
        {
            bool lDataLoaded = false;
            try
            {
                XmlNodeList nodeList = pXDoc.DocumentElement.SelectNodes("//variantGroup");

                if (nodeList.Count.Equals(0))
                {
                    lDataLoaded = false;
                    throw new InitialDataIncompleteException("Initial data did not contain variant group information! Data not loaded.");
                    ////Console.WriteLine("Initial data did not contain variant group information! Data not loaded.");

                }
                else
                {
                    foreach (XmlNode lNode in nodeList)
                    {
                        List<string> lVariantGroupVariants = new List<string>();

                        XmlNodeList variantGroupVariantsNodeList = lNode["variantRefs"].ChildNodes;
                        if (variantGroupVariantsNodeList.Count > 0)
                        {
                            foreach (XmlNode lVariantGroupVariant in variantGroupVariantsNodeList)
                            {
                                lVariantGroupVariants.Add(lVariantGroupVariant.InnerText);
                            }

                            CreateVariantGroupInstance(getXMLNodeAttributeInnerText(lNode, "variantGroupName")
                                                    , getXMLNodeAttributeInnerText(lNode, "groupCardinality")
                                                    , lVariantGroupVariants);

                            lDataLoaded = true;
                        }
                        else
                        {
                            lDataLoaded = false;
                            Console.WriteLine("Variant group defined without any variants! Data not loaded.");

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in createVariantGroupInstances");                
                Console.WriteLine(ex.Message);
            }
            return lDataLoaded;
        }

        //TODO: rename this function to show that you are loading from input
        public bool createVariantInstances(XmlDocument pXDoc)
        {
            bool lDataLoaded = false;
            try
            {
                XmlNodeList nodeList = pXDoc.DocumentElement.SelectNodes("//variant");

                if (nodeList.Count.Equals(0))
                {
                    lDataLoaded = false;
                    throw new InitialDataIncompleteException("Initial data did not contain variant information! Data not loaded.");
                    ////Console.WriteLine("Initial data did not contain variant information! Data not loaded.");

                }
                else
                {
                    foreach (XmlNode lNode in nodeList)
                    {
                        /*List<string> lVariantManufacturingOperations = new List<string>();

                        XmlNodeList variantManufacturingOperationsNodeList = lNode["variantManufacturingOps"].ChildNodes;
                        foreach (XmlNode lManufacturingOp in variantManufacturingOperationsNodeList)
                        {
                            lVariantManufacturingOperations.Add(lManufacturingOp.InnerText);
                        }

                        CreateVariantInstance(getXMLNodeAttributeInnerText(lNode, "variantName")
                                                , int.Parse(getXMLNodeAttributeInnerText(lNode, "variantIndex"))
                                                , getXMLNodeAttributeInnerText(lNode, "variantDisplayName")
                                                , lVariantManufacturingOperations);*/
                        CreateVariantInstance(getXMLNodeAttributeInnerText(lNode, "variantName")
                                                , int.Parse(getXMLNodeAttributeInnerText(lNode, "variantIndex"))
                                                , getXMLNodeAttributeInnerText(lNode, "variantDisplayName"));
                    }
                    lDataLoaded = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in createVariantInstances");                
                Console.WriteLine(ex.Message);
            }
            return lDataLoaded;
        }

        //TODO: rename this function to show that you are loading from input
        public bool createOperationInstances(XmlDocument pXDoc)
        {
            bool lDataLoaded = false;
            try
            {
                XmlNodeList nodeList = pXDoc.DocumentElement.SelectNodes("//operation");

                if (nodeList.Count.Equals(0))
                {
                    lDataLoaded = false;
                    throw new InitialDataIncompleteException("Initial data did not contain operation infor! Data not loaded.");
                    ////Console.WriteLine("Initial data did not contain operation infor! Data not loaded.");
                }
                else
                {
                    foreach (XmlNode lNode in nodeList)
                    {
                        List<string> lOperationPrecondition = new List<string>();
                        List<string> lOperationPostcondition = new List<string>();
                        List<string> lOperationRequirement = new List<string>();

                        if (lNode["requirements"] != null)
                        {
                            XmlNodeList opRequirementsList = lNode["requirements"].ChildNodes;
                            foreach (XmlNode lOpRequirement in opRequirementsList)
                            {
                                lOperationRequirement.Add(lOpRequirement.InnerText);
                            }
                        }

                        if (lNode["operationPrecondition"] != null)
                        {
                            XmlNodeList opPreconditionNodeList = lNode["operationPrecondition"].ChildNodes;
                            foreach (XmlNode lOpPrecondition in opPreconditionNodeList)
                            {
                                lOperationPrecondition.Add(lOpPrecondition.InnerText);
                            }
                        }

                        if (lNode["operationPostcondition"] != null)
                        {
                            XmlNodeList opPostconditionNodeList = lNode["operationPostcondition"].ChildNodes;
                            foreach (XmlNode lOpPostcondition in opPostconditionNodeList)
                            {
                                lOperationPostcondition.Add(lOpPostcondition.InnerText);
                            }

                        }

                        CreateOperationInstance(getXMLNodeAttributeInnerText(lNode, "operationName")
                                                , getXMLNodeAttributeInnerText(lNode, "displayName")
                                                , lOperationRequirement
                                                , lOperationPrecondition
                                                , lOperationPostcondition);
                        lDataLoaded = true;
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in createOperationInstances");
                Console.WriteLine(ex.Message);
            }
            return lDataLoaded;
        }
        
    }

    public class InitialDataIncompleteException : Exception
    {
        public InitialDataIncompleteException() : base()
        {

        }

        public InitialDataIncompleteException(string message) : base(message)
        {

        }
    }
}
