using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Xml;

namespace ProductPlatformAnalyzer
{
    //helping class not part of the xsd
    public partial class variantOperations
    {
        private variant Variant;
        private List<operation> Operations;

        public variant getVariant()
        {
            return Variant;
        }

        public void setVariant(variant pVariant)
        {
            Variant = pVariant;
        }

        public List<operation> getOperations()
        {
            return Operations;
        }

        public void setOperations(List<operation> pOperations)
        {
            Operations = pOperations;
        }

    }

    class FrameworkWrapper
    {
        private List<variantGroup> variantGroupList;
        private List<variant> variantList;
        private ArrayList constraintList;
        private List<operation> operationList;
        private List<variantOperations> variantsOperations;
        private List<string> activeOperationInstanceNamesList;
        private List<string> activeOperationNamesList;
        private List<string> inActiveOperationNamesList;
        private List<station> stationList;
        private List<resource> resourceList;
        private List<trait> traitList;

        public List<variantGroup> VariantGroupList
        {
            get { return this.variantGroupList; }
            set { this.variantGroupList = value; }
        }

        public List<variant> VariantList
        {
            get { return this.variantList; }
            set { this.variantList = value; }
        }

        public ArrayList ConstraintList
        {
            get { return this.constraintList; }
            set { this.constraintList = value; }
        }

        public List<operation> OperationList
        {
            get { return this.operationList; }
            set { this.operationList = value; }
        }

        public List<variantOperations> VariantsOperations
        {
            get { return this.variantsOperations; }
            set { this.variantsOperations = value; }
        }

        public List<string> ActiveOperationNamesList
        {
            get { return this.activeOperationNamesList; }
            set { this.activeOperationNamesList = value; }
        }

        public List<string> ActiveOperationInstanceNamesList
        {
            get { return this.activeOperationInstanceNamesList; }
            set { this.activeOperationInstanceNamesList = value; }
        }

        public List<string> InActiveOperationNamesList
        {
            get { return this.inActiveOperationNamesList; }
            set { this.inActiveOperationNamesList = value; }
        }

        public List<station> StationList
        {
            get { return this.stationList; }
            set { this.stationList = value; }
        }

        public List<resource> ResourceList
        {
            get { return this.resourceList; }
            set { this.resourceList = value; }
        }

        public List<trait> TraitList
        {
            get { return this.traitList; }
            set { this.traitList = value; }
        }

        public FrameworkWrapper()
        {
            variantList = new List<variant>();
            variantGroupList = new List<variantGroup>();
            constraintList = new ArrayList();
            operationList = new List<operation>();
            activeOperationInstanceNamesList = new List<String>();
            activeOperationNamesList = new List<String>();
            inActiveOperationNamesList = new List<string>();
            variantsOperations = new List<variantOperations>();
            stationList = new List<station>();
            resourceList = new List<resource>();
            traitList = new List<trait>();
        }

        public int getNumberOfOperations()
        {
            return OperationList.Count();
        }

        public variantOperations getVariantOperations(String pVariantName)
        {
            variantOperations tempVariantOperations = new variantOperations();
            try
            {
                foreach (variantOperations lVariantOperations in VariantsOperations)
                {
                    if (lVariantOperations.getVariant().names.Equals(pVariantName))
                        tempVariantOperations = lVariantOperations;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in getVariantOperations, pVariantName: " + pVariantName);
                Console.WriteLine(ex.Message);
            }
            return tempVariantOperations;
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

            }
            catch (Exception ex)
            {
                Console.WriteLine("error in checkOperationRequirementField, pOperationName: " + pOperation.names);
                Console.WriteLine(ex.Message);
            }
            return lCheckResult;
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
                        var lTraits = from trait in traitList
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
                    List<trait> lRequirementTraits = ExtractRequirementFieldTraits(lRequirment);

                    var resources = from resource in resourceList
                                    where resource.traits == lRequirementTraits
                                    select resource;

                    if (resources != null)
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

        public List<variantOperations> getVariantsOperations()
        {
            return VariantsOperations;
        }

        public void setVariantsOperations(List<variantOperations> pVariantsOperations)
        {
            VariantsOperations = pVariantsOperations;
        }

        public string ReturnStringElements(List<String> pList)
        {
            string lResultElements = "";

            foreach (string lElement in pList)
            {
                lResultElements += lElement;
            }
            return lResultElements;
        }


        public variant findVariantWithName(String pVariantName)
        {
            variant tempResultVariant = null;
            try
            {
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

        public int findLastVariantIndex()
        {
            int lVariantIndex = 0;
            try
            {
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
            //TODO: for now just to be simple we will make the ActiveOperationNamesList just the names of the operations
            ActiveOperationInstanceNamesList.Add(pOperationInstanceName);
        }

        public void addActiveOperationName(String pOperationName)
        {
            //TODO: for now just to be simple we will make the ActiveOperationNamesList just the names of the operations
            ActiveOperationNamesList.Add(pOperationName);
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

        public bool isActiveOperation(String pOperationName)
        {
            //In this function we have an operation name which we want to know if it is an active operation or not?
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

        public void addInActiveOperationName(String pOperationName)
        {
            //TODO: for now just to be simple we will make the ActiveOperationNamesList just the names of the operations
            InActiveOperationNamesList.Add(pOperationName);
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

        public int getVariantIndexFromActiveOperation(String pActiveOperationName)
        {
            int lVariantIndex = -1;
            try
            {
                String[] parts = pActiveOperationName.Split('_');
                if (parts[2] != null)
                    lVariantIndex = Convert.ToInt32(parts[2]);
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in getVariantIndexFromActiveOperation, pActiveOperationName: " + pActiveOperationName);
                Console.WriteLine(ex.Message);
            }

            return lVariantIndex;
        }

        public int getOperationTransitionNumberFromActiveOperation(String pActiveOperationName)
        {
            int lOpTransNum = -1;
            try
            {

                String[] parts = pActiveOperationName.Split('_');
                if (parts[3] != null)
                    lOpTransNum = Convert.ToInt32(parts[3]);

            }
            catch (Exception ex)
            {
                Console.WriteLine("error in getOperationTransitionNumberFromActiveOperation, pActiveOperationName: " + pActiveOperationName);
                Console.WriteLine(ex.Message);
            }
            return lOpTransNum;
        }

        public void addVariantsOperations(variantOperations pVariantOperations)
        {
            VariantsOperations.Add(pVariantOperations);
        }

        public void PrintDataSummary()
        {
            try
            {
                //In this function we will print a summary of the important information which is contained in the framework wrapper
                string lDataSummary = "";

                //Operations
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

                //Variants
                lDataSummary += "Variants:" + System.Environment.NewLine;
                foreach (variant lVariant in VariantList)
                {
                    lDataSummary += "Variant Name: " + lVariant.names + System.Environment.NewLine;
                    lDataSummary += "Variant Index: " + lVariant.index + System.Environment.NewLine;
                    lDataSummary += "Variant Display Name: " + lVariant.displayName + System.Environment.NewLine;
                    foreach (operation lManOperation in lVariant.manOperations)
                        lDataSummary += "Variant Manufacturing Operation: " + lManOperation.names + System.Environment.NewLine;
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

                System.IO.File.WriteAllText("C:/Users/amirho/Desktop/Output/Debug/DataSummary.txt", lDataSummary);

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

//        public void CreateVariantInstance(String pName, int pIndex, String pDisplayName, List<String> pManufacturingOperation)
        public void CreateVariantInstance(String pName, int pIndex, String pDisplayName)
        {
            try
            {
                variant tempVariant = new variant();
                tempVariant.names = pName;
                tempVariant.index = pIndex;
                tempVariant.displayName = pDisplayName;
                /*if (pManufacturingOperation != null)
                {
                    List<operation> lManOperations = new List<operation>();
                    foreach (String lManufacturingOperationName in pManufacturingOperation)
                    {
                        lManOperations.Add(findOperationWithName(lManufacturingOperationName));
                    }
                    tempVariant.manOperations = lManOperations;
                }*/
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

        public void CreateVariantGroupInstance(String pName, String pGroupCardinality, List<String> pVariantList)
        {
            try
            {
                variantGroup tempVariantGroup = new variantGroup();
                tempVariantGroup.names = pName;
                tempVariantGroup.gCardinality = pGroupCardinality;
                if (pVariantList != null)
                {
                    List<variant> tempVariantList = new List<variant>();
                    foreach (String lVariantName in pVariantList)
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

        private void CreateVariantOperationMappingInstance(String pVariantName, List<string> pOperationList)
        {
            try
            {
                variantOperations lVariantOperations = new variantOperations();
                lVariantOperations.setVariant(findVariantWithName(pVariantName));
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

        public void LoadInitialDataFromXMLFile(string pFilePath)
        {
            try
            {
                //new instance of xdoc
                XmlDocument xDoc = new XmlDocument();

                //First load the XML file from the file path
                xDoc.Load(pFilePath);

                createOperationInstances(xDoc);
                createVariantInstances(xDoc);
                createVariantGroupInstances(xDoc);
                createConstraintInstances(xDoc);
                createVariantOperationInstances(xDoc);
                //createStationInstances(xDoc);
                createTraitInstances(xDoc);
                createResourceInstances(xDoc);
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in LoadInitialDataFromXMLFile, FilePath: " + pFilePath);                
                Console.WriteLine(ex.Message);
            }
        }

        private void createTraitInstances(XmlDocument pXDoc)
        {
            try
            {
                XmlNodeList nodeList = pXDoc.DocumentElement.SelectNodes("//trait");

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
                        lAttributes.Add(new Tuple<string,string>(getXMLNodeAttributeInnerText(lAttribute, "attributeType")
                                        , getXMLNodeAttributeInnerText(lAttribute, "attributeName")));
                    }

                    createTraitInstance(getXMLNodeAttributeInnerText(lNode, "traitName")
                                                            , lInheritTraits
                                                            , lAttributes);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in createTraitInstances");
                Console.WriteLine(ex.Message);
            }
        }

        private void createResourceInstances(XmlDocument pXDoc)
        {
            try
            {
                XmlNodeList nodeList = pXDoc.DocumentElement.SelectNodes("//resource");

                foreach (XmlNode lNode in nodeList)
                {
                    List<Tuple<string, string, string>> lAttributes = new List<Tuple<string,string,string>>();
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
                        lAttributes.Add(new Tuple<string,string,string>(getXMLNodeAttributeInnerText(lAttribute, "attributeName")
                                                                        ,getXMLNodeAttributeInnerText(lAttribute, "attributeType")
                                                                        ,getXMLNodeAttributeInnerText(lAttribute, "attributeValue")));
                    }


                    CreateResourceInstance(getXMLNodeAttributeInnerText(lNode, "resourceName")
                                                            , lTraits
                                                            , lAttributes);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in createResourceInstances");
                Console.WriteLine(ex.Message);
            }
        }

        private trait findTraitWithName(string pTraitName)
        {
            trait lResultTrait = new trait();
            try
            {
                List<trait> lTempResultTrait = (from trait in traitList
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

        private void CreateStationInstance(String pName, List<string> pStationResources)
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

        public void createVariantOperationInstances(XmlDocument pXDoc)
        {
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


                    CreateVariantOperationMappingInstance(getXMLNodeAttributeInnerText(lNode,"variantRefs")
                                                            , lVariantOperations);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in createVariantOperationInstances");                
                Console.WriteLine(ex.Message);
            }
        }

        /*public void createOperationResourceInstances(XmlDocument pXDoc)
        {
            try
            {
                XmlNodeList nodeList = pXDoc.DocumentElement.SelectNodes("//operationResourceMapping");

                foreach (XmlNode lNode in nodeList)
                {
                    List<string> lOperationResources = new List<string>();

                    XmlNodeList operationResourcesNodeList = lNode["resourceRefs"].ChildNodes;
                    foreach (XmlNode lOperationResource in operationResourcesNodeList)
                    {
                        lOperationResources.Add(lOperationResource.InnerText);
                    }


                    CreateOperationResourceMappingInstance(getXMLNodeAttributeInnerText(lNode, "operationRefs")
                                                            , lOperationResources);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in createOperationResourceInstances");
                Console.WriteLine(ex.Message);
            }
        }*/

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

        public void createConstraintInstances(XmlDocument pXDoc)
        {
            try
            {
                XmlNodeList nodeList = pXDoc.DocumentElement.SelectNodes("//constraint");

                foreach (XmlNode lNode in nodeList)
                    addConstraint(getXMLNodeAttributeInnerText(lNode,"logic"));
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in createConstraintInstances");                
                Console.WriteLine(ex.Message);
            }
        }

        public void createVariantGroupInstances(XmlDocument pXDoc)
        {
            try
            {
                XmlNodeList nodeList = pXDoc.DocumentElement.SelectNodes("//variantGroup");

                foreach (XmlNode lNode in nodeList)
                {
                    List<string> lVariantGroupVariants = new List<string>();

                    XmlNodeList variantGroupVariantsNodeList = lNode["variantRefs"].ChildNodes;
                    foreach (XmlNode lVariantGroupVariant in variantGroupVariantsNodeList)
                    {
                        lVariantGroupVariants.Add(lVariantGroupVariant.InnerText);
                    }

                    CreateVariantGroupInstance(getXMLNodeAttributeInnerText(lNode,"variantGroupName")
                                            , getXMLNodeAttributeInnerText(lNode,"groupCardinality")
                                            , lVariantGroupVariants);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in createVariantGroupInstances");                
                Console.WriteLine(ex.Message);
            }
        }

        public void createVariantInstances(XmlDocument pXDoc)
        {
            try
            {
                XmlNodeList nodeList = pXDoc.DocumentElement.SelectNodes("//variant");

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
                                            , int.Parse(getXMLNodeAttributeInnerText(lNode,"variantIndex"))
                                            , getXMLNodeAttributeInnerText(lNode,"variantDisplayName"));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in createVariantInstances");                
                Console.WriteLine(ex.Message);
            }
        }

        public void createOperationInstances(XmlDocument pXDoc)
        {
            try
            {
                XmlNodeList nodeList = pXDoc.DocumentElement.SelectNodes("//operation");

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
                                            , getXMLNodeAttributeInnerText(lNode,"displayName")
                                            , lOperationRequirement
                                            , lOperationPrecondition
                                            , lOperationPostcondition);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in createOperationInstances");
                Console.WriteLine(ex.Message);
            }
        }

    }
}
