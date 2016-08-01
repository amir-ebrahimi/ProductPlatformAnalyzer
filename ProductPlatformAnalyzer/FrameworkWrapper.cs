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

    public class FrameworkWrapper
    {
        private List<variantGroup> VariantGroupList;
        private List<variant> VariantList;
        private ArrayList ConstraintList;
        private List<operation> OperationList;
        private List<variantOperations> VariantsOperations;
        private List<String> ActiveOperationList;
        private List<String> InActiveOperationList;
        private List<station> StationList;
        private List<resource> ResourceList;
        private List<operationResources> OperationResources;

        public FrameworkWrapper()
        {
            VariantList = new List<variant>();
            VariantGroupList = new List<variantGroup>();
            ConstraintList = new ArrayList();
            OperationList = new List<operation>();
            ActiveOperationList = new List<String>();
            InActiveOperationList = new List<string>();
            VariantsOperations = new List<variantOperations>();
            StationList = new List<station>();
            ResourceList = new List<resource>();
            OperationResources = new List<operationResources>();
        }

        public int getNumberOfOperations()
        {
            return OperationList.Count();
        }

        public List<variantGroup> getVariantGroupList()
        {
            return VariantGroupList;
        }

        public void setVariantGroupList(List<variantGroup> pArrayList)
        {
            VariantGroupList = pArrayList;
        }

        public List<variant> getVariantList()
        {
            return VariantList;
        }

        public void setVariantList(List<variant> pArrayList)
        {
            VariantList = pArrayList;
        }

        public ArrayList getConstraintList()
        {
            return ConstraintList;
        }

        public void setConstraintList(ArrayList pConstraintList)
        {
            ConstraintList = pConstraintList;
        }

        public List<operation> getOperationList()
        {
            return OperationList;
        }

        public void setOperationList(List<operation> pOperationList)
        {
            OperationList = pOperationList;
        }

        public List<String> getActiveOperationList()
        {
            return ActiveOperationList;
        }

        public List<String> getInActiveOperationList()
        {
            return InActiveOperationList;
        }

        public List<station> getStationList()
        {
            return StationList;
        }

        public void setStationList(List<station> pArrayList)
        {
            StationList = pArrayList;
        }

        public List<resource> getResourceList()
        {
            return ResourceList;
        }

        public void setResourceList(List<resource> pArrayList)
        {
            ResourceList = pArrayList;
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

        public List<String> getActiveOperationList(int pState)
        {
            List<String> tempActiveOperationList = new List<string>();
            try
            {
                foreach (String operationName in ActiveOperationList)
                {
                    if (getOperationStateFromOperationName(operationName).Equals(pState.ToString()))
                        tempActiveOperationList.Add(operationName);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in getActiveOperationList, pState: " + pState);
                Console.WriteLine(ex.Message);
            }
            return tempActiveOperationList;
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

        public void setActiveOperationList(List<String> pActiveOperationList)
        {
            ActiveOperationList = pActiveOperationList;
        }

        public void setInActiveOperationList(List<String> pInActiveOperationList)
        {
            InActiveOperationList = pInActiveOperationList;
        }

        public List<string> getvariantInstancesForOperation(string op)
        {
            List<string> instances = new List<string>();
            string[] opParts = new string[4];
            foreach(string iOp in ActiveOperationList)
            {
                opParts = iOp.Split('_');
                if (String.Equals(opParts[0], op) && String.Equals(opParts[1], "F") && String.Equals(opParts[3], "0"))
                    instances.Add(opParts[2]);
            }
            return instances;
        }

        public List<variantOperations> getVariantsOperations()
        {
            return VariantsOperations;
        }

        public void setVariantsOperations(List<variantOperations> pVariantsOperations)
        {
            VariantsOperations = pVariantsOperations;
        }

        public List<operationResources> getOperationResources()
        {
            return OperationResources;
        }

        public void setOperationResources(List<operationResources> pOperationResources)
        {
            OperationResources = pOperationResources;
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

        public void addActiveOperation(String pOperationName)
        {
            //TODO: for now just to be simple we will make the ActiveOperationList just the names of the operations
            ActiveOperationList.Add(pOperationName);
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

        public bool isActiveOperation(String pOperationName)
        {
            //In this function we have an operation name which we want to know if it is an active operation or not?
            bool lResult = false;
            try
            {
                foreach (String lActiveOperationName in ActiveOperationList)
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

        public void addInActiveOperation(String pOperationName)
        {
            //TODO: for now just to be simple we will make the ActiveOperationList just the names of the operations
            InActiveOperationList.Add(pOperationName);
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

        public string getVariantGroup(string varName)
        {
            variant var = findVariantWithName(varName);
            variantGroup varGroup = getVariantGroup(var);
            if(varGroup != null)
                return varGroup.names;
            return null;
        }

        public variantGroup getVariantGroup(variant var)
        {
            foreach (variantGroup vg in VariantGroupList)
            {

                foreach (variant v in vg.variant)
                {
                    if (v.Equals(var))
                        return vg;
                }
            }
            return null;
        }

        public int getOperationTransitionNumberFromActiveOperation(String pActiveOperationName)
        {
            int lOpTransNum = -1;
            try
            {

                String[] parts = pActiveOperationName.Split('_');
                if (parts.Length == 4)
                    lOpTransNum = Convert.ToInt32(parts[3]);

            }
            catch (Exception ex)
            {
                Console.WriteLine("error in getOperationTransitionNumberFromActiveOperation, pActiveOperationName: " + pActiveOperationName);
                Console.WriteLine(ex.Message);
            }
            return lOpTransNum;
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
            List<string> con ;
            operation op = findOperationWithName(opName);
            con =  new List<string>(op.precondition);
            return con;
        }

        public List<string> getPostconditionForOperation(string opName)
        {
            List<string> con;
            operation op = findOperationWithName(opName);
            con = new List<string>(op.postcondition);
            return con;
        }
        public void addVariantsOperations(variantOperations pVariantOperations)
        {
            VariantsOperations.Add(pVariantOperations);
        }

        public void addOperationResources(operationResources pOperationResources)
        {
            OperationResources.Add(pOperationResources);
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

        public void CreateOperationInstance(String pName, String pDisplayName, List<String> pPreconditions, List<String> pPostconditions)
        {
            try
            {
                operation tempOperation = new operation();
                tempOperation.names = pName;
                tempOperation.displayName = pDisplayName;
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
                        tempOperations.Add(findOperationWithName(lOperationName));
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

        private void CreateOperationResourceMappingInstance(String pOperationName, List<string> pResourceList)
        {
            try
            {
                operationResources lOperationResources = new operationResources();
                lOperationResources.setOperationRef(findOperationWithName(pOperationName));
                if (pResourceList != null)
                {
                    List<resource> tempResources = new List<resource>();
                    foreach (String lResourceName in pResourceList)
                    {
                        tempResources.Add(findResourceWithName(lResourceName));
                    }
                    lOperationResources.setResourceRefs(tempResources);
                }
                addOperationResources(lOperationResources);
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in CreateOperationResourceMappingInstance, pOperationName: " + pOperationName
                                                                + " ,pResourceList: " + ReturnStringElements(pResourceList));
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
                createStationInstances(xDoc);
                createResourceInstances(xDoc);
                createOperationResourceInstances(xDoc);
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in LoadInitialDataFromXMLFile, FilePath: " + pFilePath);                
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
                    CreateResourceInstance(getXMLNodeAttributeInnerText(lNode, "resourceName")
                                                            , getXMLNodeAttributeInnerText(lNode, "resourceAbility"));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in createResourceInstances");
                Console.WriteLine(ex.Message);
            }
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

        public void CreateResourceInstance(String pName, String pAbility)
        {
            try
            {
                resource tempResource = new resource();
                tempResource.names = pName;
                tempResource.ability = pAbility;
                addResource(tempResource);
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in CreateResourceInstance, pName: " + pName
                                                                + " ,pAbility: " + pAbility);
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

        public void createOperationResourceInstances(XmlDocument pXDoc)
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
