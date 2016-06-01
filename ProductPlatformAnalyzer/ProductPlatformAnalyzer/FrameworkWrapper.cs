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
        private List<variantGroup> VariantGroupList;
        private List<variant> VariantList;
        private ArrayList ConstraintList;
        private List<operation> OperationList;
        private List<variantOperations> VariantsOperations;
        private List<String> ActiveOperationList;
        private List<String> InActiveOperationList;

        public FrameworkWrapper()
        {
            VariantList = new List<variant>();
            VariantGroupList = new List<variantGroup>();
            ConstraintList = new ArrayList();
            OperationList = new List<operation>();
            ActiveOperationList = new List<String>();
            InActiveOperationList = new List<string>();
            VariantsOperations = new List<variantOperations>();
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

        public void addActiveOperation(String pOperationName)
        {
            //TODO: for now just to be simple we will make the ActiveOperationList just the names of the operations
            ActiveOperationList.Add(pOperationName);
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

        public void LoadInitialDataFromXMLFile (string pFilePath)
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
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in LoadInitialDataFromXMLFile, FilePath: " + pFilePath);                
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

        /*public void createTestData1()
        {
            //no deadlock, just one config, one operation
            CreateOperationInstance("Scan-barcode", "Scan barcode", null, null);

            CreateVariantInstance("frame-rigid", 1, "frame-rigid", new List<string> { "Scan-barcode" });

            CreateVariantGroupInstance("frame", "choose exactly one", new List<string> { "frame-rigid" });

            addConstraint("frame-rigid");

            CreateVariantOperationMappingInstance("frame-rigid", new List<string> { "Scan-barcode" });
        }

        public void createTestData2()
        {
            //no deadlock, just one config, two operation
            CreateOperationInstance("Scan-barcode", "Scan barcode", null, null);
            CreateOperationInstance("Mount", "Mount", new List<string> { "Scan-barcode_F_1_2" }, new List<string> { "Scan-barcode_F_1_2" });

            CreateVariantInstance("frame-rigid", 1, "frame-rigid", new List<string> { "Scan-barcode", "Mount" });

            CreateVariantGroupInstance("frame", "choose exactly one", new List<string> { "frame-rigid" });

            addConstraint("frame-rigid");

            CreateVariantOperationMappingInstance("frame-rigid", new List<string> { "Scan-barcode", "Mount" });
        }

        public void createTestData3()
        {
            //one deadlock in the first transition, no config, two operation
            CreateOperationInstance("Scan-barcode", "Scan barcode", null, null);
            CreateOperationInstance("Mount", "Mount", new List<string> { "Scan-barcode_F_2_0" }, new List<string> { "Scan-barcode_F_2_0" });

            CreateVariantInstance("frame-rigid", 1, "frame-rigid", new List<string> { "Mount" });
            CreateVariantInstance("frame-tractor", 2, "frame-tractor", new List<string> { "Scan-barcode" });

            CreateVariantGroupInstance("frame", "choose exactly one", new List<string> { "frame-rigid" , "frame-tractor" });

            addConstraint("frame-rigid");

            CreateVariantOperationMappingInstance("frame-rigid", new List<string> { "Mount" });
            CreateVariantOperationMappingInstance("frame-tractor", new List<string> { "Scan-barcode" });
        }

        public void createTestData4()
        {
            //one deadlock, one precondition false, three operation
            //Deadlock in the first transition
            CreateOperationInstance("Scan-barcode", "Scan barcode", new List<string> { "Mount_F_2_0" }, new List<string> { "Mount_F_2_0" });
            CreateOperationInstance("Mount", "Mount", null, null);
            CreateOperationInstance("Test_frame_rigid", "Test frame rigid", new List<string> { "Scan-barcode_F_1_2" }, new List<string> { "Scan-barcode_F_1_2" });

            CreateVariantInstance("frame-rigid", 1, "frame-rigid", new List<string> { "Scan-barcode", "Test_frame_rigid" });
            CreateVariantInstance("frame-tractor", 2, "frame-tractor", new List<string> { "Mount" });

            CreateVariantGroupInstance("frame", "choose exactly one", new List<string> { "frame-rigid", "frame-tractor" });

            addConstraint("frame-rigid");

            CreateVariantOperationMappingInstance("frame-rigid", new List<string> { "Scan-barcode", "Test_frame_rigid" });
            CreateVariantOperationMappingInstance("frame-tractor", new List<string> { "Mount" });
        }

        public void createTestData5()
        {
            //one deadlock, one precondition false, three operation
            //Deadlock in the third transition
            CreateOperationInstance("Scan-barcode", "Scan barcode", null, null);
            CreateOperationInstance("Mount", "Mount", null, null);
            CreateOperationInstance("Test_frame_rigid", "Test frame rigid", new List<string> { "Mount_F_2_2" }, new List<string> { "Mount_F_2_2" });

            CreateVariantInstance("frame-rigid", 1, "frame-rigid", new List<string> { "Scan-barcode", "Test_frame_rigid" });
            CreateVariantInstance("frame-tractor", 2, "frame-tractor", new List<string> { "Mount" });

            CreateVariantGroupInstance("frame", "choose exactly one", new List<string> { "frame-rigid", "frame-tractor" });

            addConstraint("frame-rigid");

            CreateVariantOperationMappingInstance("frame-rigid", new List<string> { "Scan-barcode", "Test_frame_rigid" });
            CreateVariantOperationMappingInstance("frame-tractor", new List<string> { "Mount" });
        }

        public void createTestData6()
        {
            //Old Volvo Variant Matrix
            CreateOperationInstance("Load-AGV", "Load-AGV", null, null);
            CreateOperationInstance("Secure-Part-ID", "Secure-Part-ID", null, null);
            CreateOperationInstance("Place-and-Secure-Cab-to-Frame", "Place-and-Secure-Cab-to-Frame", new List<string> { "Secure-Part-ID" }, null);
            CreateOperationInstance("Drop-engine-and-Attach-to-Frame", "Drop-engine-and-Attach-to-Frame", new List<string> { "Secure-Part-ID" }, null);
            CreateOperationInstance("Place-and-attach-Single-axle", "Place-and-attach-Single-axle", new List<string> { "Secure-Part-ID" }, null);
            CreateOperationInstance("Place-and-attach-Double-axle", "Place-and-attach-Double-axle", new List<string> { "Secure-Part-ID" }, null);
            CreateOperationInstance("Attach-Accessory-Plate-to-Frame", "Attach-Accessory-Plate-to-Frame", new List<string> { "Secure-Part-ID" }, null);
            CreateOperationInstance("Attach-Fuel-tank-to-Frame", "Attach-Fuel-tank-to-Frame", new List<string> { "Secure-Part-ID" }, null);
            CreateOperationInstance("Attach-Fifth-Wheel-to-Frame", "Attach-Fifth-Wheel-to-Frame", new List<string> { "Secure-Part-ID" }, null);
            CreateOperationInstance("Attach-Side-Panel", "Attach-Side-Panel", new List<string> { "Secure-Part-ID" }, null);
            CreateOperationInstance("Attach-Side-Cover", "Attach-Side-Cover", new List<string> { "Secure-Part-ID" }, null);
            CreateOperationInstance("Place-and-Secure-Heavy-Horn-to-Cab", "Place-and-Secure-Heavy-Horn-to-Cab", new List<string> { "Secure-Part-ID" }, null);
            CreateOperationInstance("Place-and-Secure-Light-Horn-to-Cab", "Place-and-Secure-Light-Horn-to-Cab", new List<string> { "Secure-Part-ID" }, null);
            CreateOperationInstance("Setup-Heavy-Engine-Sound", "Setup-Heavy-Engine-Sound", new List<string> { "Secure-Part-ID" }, null);
            CreateOperationInstance("Setup-Light-Engine-Sound", "Setup-Light-Engine-Sound", new List<string> { "Secure-Part-ID" }, null);
            CreateOperationInstance("Place-and-Secure-Front-Lights-to-Cab", "Place-and-Secure-Front-Lights-to-Cab", new List<string> { "Secure-Part-ID" }, null);
            CreateOperationInstance("Place-and-Secure-Interior-Lights-to-Cab", "Place-and-Secure-Interior-Lights-to-Cab", new List<string> { "Secure-Part-ID" }, null);
            CreateOperationInstance("Place-and-Secure-Indicator-Lights-to-Cab", "Place-and-Secure-Indicator-Lights-to-Cab", new List<string> { "Secure-Part-ID" }, null);
            CreateOperationInstance("Place-and-Secure-Working-Lights-to-Cab", "Place-and-Secure-Working-Lights-to-Cab", new List<string> { "Secure-Part-ID" }, null);
            CreateOperationInstance("Place-and-Secure-Beamer-Lights-to-Cab", "Place-and-Secure-Beamer-Lights-to-Cab", new List<string> { "Secure-Part-ID" }, null);
            CreateOperationInstance("Place-and-Secure-Rotational-Lights-to-Cab", "Place-and-Secure-Rotational-Lights-to-Cab", new List<string> { "Secure-Part-ID" }, null);

            CreateVariantInstance("frame-rigid", 1, "frame-rigid", new List<string> { "Load-AGV" });
            CreateVariantInstance("frame-tractor", 2, "frame-tractor", new List<string> { "Load-AGV" });
            CreateVariantInstance("Cab_red", 3, "Cab_red", new List<string> { "Secure-Part-ID", "Place-and-Secure-Cab-to-Frame" });
            CreateVariantInstance("Cab_yellow", 4, "Cab_yellow", new List<string> { "Secure-Part-ID", "Place-and-Secure-Cab-to-Frame" });
            CreateVariantInstance("Engine", 5, "Engine", new List<string> { "Secure-Part-ID", "Drop-engine-and-Attach-to-Frame" });
            CreateVariantInstance("FWU_Single_wheel_axle", 6, "FWU_Single_wheel_axle", new List<string> { "Secure-Part-ID", "Place-and-attach-Single-axle" });
            CreateVariantInstance("FRWU_Single_wheel_axle", 7, "FRWU_Single_wheel_axle", new List<string> { "Secure-Part-ID", "Place-and-attach-Single-axle" });
            CreateVariantInstance("FRWU_Double_wheel_axle", 8, "FRWU_Double_wheel_axle", new List<string> { "Secure-Part-ID", "Place-and-attach-Double-axle" });
            CreateVariantInstance("SRWU_Single_wheel_axle", 9, "FRWU_Single_wheel_axle", new List<string> { "Secure-Part-ID", "Place-and-attach-Single-axle" });
            CreateVariantInstance("SRWU_Double_wheel_axle", 10, "SRWU_Double_wheel_axle", new List<string> { "Secure-Part-ID", "Place-and-attach-Double-axle" });
            CreateVariantInstance("LAP_Accessories_plate", 11, "LAP_Accessories_plate", new List<string> { "Secure-Part-ID", "Attach-Accessory-Plate-to-Frame" });
            CreateVariantInstance("LAP_Fuel_tank", 12, "LAP_Fuel_tank", new List<string> { "Secure-Part-ID", "Attach-Fuel-tank-to-Frame" });
            CreateVariantInstance("RAP_Accessories_plate", 13, "RAP_Accessories_plate", new List<string> { "Secure-Part-ID", "Attach-Accessory-Plate-to-Frame" });
            CreateVariantInstance("RAP_Fuel_tank", 14, "RAP_Fuel_tank", new List<string> { "Secure-Part-ID", "Attach-Fuel-tank-to-Frame" });
            CreateVariantInstance("Fifth_wheel", 15, "Fifth_wheel", new List<string> { "Secure-Part-ID", "Attach-Fifth-Wheel-to-Frame" });
            CreateVariantInstance("Side_panel_red", 16, "Side_panel_red", new List<string> { "Secure-Part-ID", "Attach-Side-Panel" });
            CreateVariantInstance("Side_panel_yellow", 17, "Side_panel_yellow", new List<string> { "Secure-Part-ID", "Attach-Side-Panel" });
            CreateVariantInstance("Grey_side_cover", 18, "Grey_side_cover", new List<string> { "Secure-Part-ID", "Attach-Side-Cover" });
            CreateVariantInstance("Heavy_horn", 19, "Heavy_horn", new List<string> { "Secure-Part-ID", "Place-and-Secure-Heavy-Horn-to-Cab" });
            CreateVariantInstance("Light_horn", 20, "Light_horn", new List<string> { "Secure-Part-ID", "Place-and-Secure-Light-Horn-to-Cab" });
            CreateVariantInstance("Heavy_engine", 21, "Heavy_engine", new List<string> { "Secure-Part-ID", "Setup-Heavy-Engine-Sound" });
            CreateVariantInstance("Light_engine", 22, "Light_engine", new List<string> { "Secure-Part-ID", "Setup-Light-Engine-Sound" });
            CreateVariantInstance("Interior_lights", 23, "Interior_lights", new List<string> { "Secure-Part-ID", "Place-and-Secure-Interior-Lights-to-Cab" });
            CreateVariantInstance("Working_lights", 24, "Working_lights", new List<string> { "Secure-Part-ID", "Place-and-Secure-Working-Lights-to-Cab" });
            CreateVariantInstance("Beamer_lights", 25, "Beamer_lights", new List<string> { "Secure-Part-ID", "Place-and-Secure-Beamer-Lights-to-Cab" });
            CreateVariantInstance("Rotational_lights", 26, "Rotational_lights", new List<string> { "Secure-Part-ID", "Place-and-Secure-Rotational-Lights-to-Cab" });
            CreateVariantInstance("Front_lights", 27, "Front_lights", new List<string> { "Secure-Part-ID", "Place-and-Secure-Front-Lights-to-Cab" });
            CreateVariantInstance("Indicator_lights", 28, "Indicator_lights", new List<string> { "Secure-Part-ID", "Place-and-Secure-Indicator-Lights-to-Cab" });

            CreateVariantGroupInstance("frame", "choose exactly one", new List<string> { "frame-rigid", "frame-tractor" });
            CreateVariantGroupInstance("Cab", "choose exactly one", new List<string> { "Cab_red", "Cab_yellow" });
            CreateVariantGroupInstance("Powertrain", "choose exactly one", new List<string> { "Engine" });
            CreateVariantGroupInstance("Front_wheel_unit", "choose exactly one", new List<string> { "FWU_Single_wheel_axle" });
            CreateVariantGroupInstance("First_rear_wheel_unit", "choose exactly one", new List<string> { "FRWU_Single_wheel_axle", "FRWU_Double_wheel_axle" });
            CreateVariantGroupInstance("Second_rear_wheel_unit", "choose exactly one", new List<string> { "SRWU_Single_wheel_axle", "SRWU_Double_wheel_axle" });
            CreateVariantGroupInstance("Left_accessory_position", "choose exactly one", new List<string> { "LAP_Accessories_plate", "LAP_Fuel_tank" });
            CreateVariantGroupInstance("Right_accessory_position", "choose exactly one", new List<string> { "RAP_Accessories_plate", "RAP_Fuel_tank" });
            CreateVariantGroupInstance("Other", "choose at least one", new List<string> { "Fifth_wheel", "Side_panel_red", "Side_panel_yellow", "Grey_side_cover" });
            CreateVariantGroupInstance("Horn_Sound", "choose exactly one", new List<string> { "Heavy_horn", "Light_horn" });
            CreateVariantGroupInstance("Engine_Sound", "choose exactly one", new List<string> { "Heavy_engine", "Light_engine" });
            CreateVariantGroupInstance("Lights", "choose at least one", new List<string> { "Interior_lights", "Working_lights", "Beamer_lights", "Rotational_lights", "Front_lights", "Indicator_lights" });

            ////addConstraint("Front_lights");
            ////addConstraint("not Engine");
            ////addConstraint("or Indicator_lights FWU_Single_wheel_axle");
            ////addConstraint("and FWU_Single_wheel_axle Indicator_lights");
        	//[(Heavy_engine && Heavy_horn) || (Light_engine && Light_horn)]
            addConstraint("or (and Heavy_engine Heavy_horn) (and Light_engine Light_horn)");
            //[(Heavy_engine && (Beamer_lights || Rotational_lights)) || (! Heavy_engine && (! Beamer_lights && ! Rotational_lights))]
            ////addConstraint("or (and Heavy_engine (or Beamer_lights Rotational_lights)) (and (not Heavy_engine) (and (not Beamer_lights) (not Rotational_lights)))");
            //[(Beamer_lights && ! Rotational_lights) || ((! Beamer_lights && Rotational_lights) || (! Beamer_lights && ! Rotational_lights))]
            ////addConstraint("or (and Beamer_lights (not Rotational_lights)) (or (and (not Beamer_lights) Rotational_lights) (and (not Beamer_lights) (not Rotational_lights)))");
            //[(Cab_red && Interior_lights) || (! Cab_red && ! Interior_lights)]
            ////addConstraint("or (and Cab_red Interior_lights) (and (not Cab_red) (not Interior_lights))");
            //[(Frame_tractor && Light_engine) || (Frame_rigid && Heavy_engine)]
            ////addConstraint("or (and Frame_tractor Light_engine) (and Frame_rigid Heavy_engine)");
            //[(SRWU_Single_wheel_axle && Beamer_lights) || ((SRWU_Double_wheel_axle && Rotational_lights) || (! SRWU_Single_wheel_axle && ! SRWU_Double_wheel_axle))]
            ////addConstraint("or (and SRWU_Single_wheel_axle Beamer_lights) (or (and SRWU_Double_wheel_axle Rotational_lights) (and (not SRWU_Single_wheel_axle) (not SRWU_Double_wheel_axle)))");
            //[(Working_lights && LAP_Accessories_plate) || (! Working_lights && ! LAP_Accessories_plate)]
            ////addConstraint("or (and Working_lights LAP_Accessories_plate) (and (not Working_lights) (not LAP_Accessories_plate))");
            //[(Frame_tractor && FRWU_Double_wheel_axle) || (Frame_rigid && (FRWU_Single_wheel_axle || FRWU_Double_wheel_axle))]
            ////addConstraint("or (and Frame_tractor FRWU_Double_wheel_axle) (and Frame_rigid (or FRWU_Single_wheel_axle FRWU_Double_wheel_axle))");
            //[(SRWU_Single_wheel_axle && FRWU_Double_wheel_axle) || ((SRWU_Double_wheel_axle && (FRWU_Single_wheel_axle || FRWU_Double_wheel_axle)) || (! SRWU_Single_wheel_axle && ! SRWU_Double_wheel_axle))]
            ////addConstraint("or (and SRWU_Single_wheel_axle FRWU_Double_wheel_axle) (or (and SRWU_Double_wheel_axle (or FRWU_Single_wheel_axle FRWU_Double_wheel_axle)) (and (not  SRWU_Single_wheel_axle) (not SRWU_Double_wheel_axle))");
            //[(Frame_tractor && ((Cab_red && (Side_panel_red && ! Side_panel_yellow)) || (Cab_yellow && (Side_panel_yellow && ! Side_panel_red)))) || ((Frame_rigid && ! Side_panel_red) && ! Side_panel_yellow)]
            ////addConstraint("or (and Frame_tractor (or (and Cab_red (and Side_panel_red (not Side_panel_yellow))) (and Cab_yellow (and Side_panel_yellow (not Side_panel_red))))) (and (and Frame_rigid (not Side_panel_red)) (not Side_panel_yellow))");
            //[(Frame_rigid && (SRWU_Single_wheel_axle || SRWU_Double_wheel_axle)) || (Frame_tractor && (! SRWU_Single_wheel_axle && ! SRWU_Double_wheel_axle))]
            ////addConstraint("or (and Frame_rigid (or SRWU_Single_wheel_axle SRWU_Double_wheel_axle)) (and Frame_tractor (and (not SRWU_Single_wheel_axle) (not SRWU_Double_wheel_axle)))");
            //[(Frame_rigid && ! Fifth_wheel) || (Frame_tractor && Fifth_wheel)]
            ////addConstraint("or (and Frame_rigid (not Fifth_wheel)) (and Frame_tractor Fifth_wheel)");
            //[(Frame_tractor && ! Grey_side_cover) || (Frame_rigid && Grey_side_cover)]
            ////addConstraint("or (and Frame_tractor (not Grey_side_cover)) (and Frame_rigid Grey_side_cover)");
            //[(Frame_rigid && (Cab_red || Cab_yellow)) || (Frame_tractor && (Cab_red || Cab_yellow))]
            ////addConstraint("or (and Frame_rigid (or Cab_red Cab_yellow)) (and Frame_tractor (or Cab_red Cab_yellow))");
            //[(LAP_Accessories_plate && RAP_Fuel_tank) || (RAP_Accessories_plate && LAP_Fuel_tank)]
            ////addConstraint("or (and LAP_Accessories_plate RAP_Fuel_tank) (and RAP_Accessories_plate LAP_Fuel_tank)");
    
        
            CreateVariantOperationMappingInstance("frame-rigid", new List<string> { "Load-AGV" });
            CreateVariantOperationMappingInstance("frame-tractor", new List<string> { "Load-AGV" });
            CreateVariantOperationMappingInstance("Cab_red", new List<string> { "Secure-Part-ID", "Place-and-Secure-Cab-to-Frame" });
            CreateVariantOperationMappingInstance("Cab_yellow", new List<string> { "Secure-Part-ID", "Place-and-Secure-Cab-to-Frame" });
            CreateVariantOperationMappingInstance("Engine", new List<string> { "Secure-Part-ID", "Drop-engine-and-Attach-to-Frame" });
            CreateVariantOperationMappingInstance("FWU_Single_wheel_axle", new List<string> { "Secure-Part-ID", "Place-and-attach-Single-axle" });
            CreateVariantOperationMappingInstance("FRWU_Single_wheel_axle", new List<string> { "Secure-Part-ID", "Place-and-attach-Single-axle" });
            CreateVariantOperationMappingInstance("FRWU_Double_wheel_axle", new List<string> { "Secure-Part-ID", "Place-and-attach-Double-axle" });
            CreateVariantOperationMappingInstance("SRWU_Single_wheel_axle", new List<string> { "Secure-Part-ID", "Place-and-attach-Single-axle" });
            CreateVariantOperationMappingInstance("SRWU_Double_wheel_axle", new List<string> { "Secure-Part-ID", "Place-and-attach-Double-axle" });
            CreateVariantOperationMappingInstance("LAP_Accessories_plate", new List<string> { "Secure-Part-ID", "Attach-Accessory-Plate-to-Frame" });
            CreateVariantOperationMappingInstance("LAP_Fuel_tank", new List<string> { "Secure-Part-ID", "Attach-Fuel-tank-to-Frame" });
            CreateVariantOperationMappingInstance("RAP_Accessories_plate", new List<string> { "Secure-Part-ID", "Attach-Accessory-Plate-to-Frame" });
            CreateVariantOperationMappingInstance("RAP_Fuel_tank", new List<string> { "Secure-Part-ID", "Attach-Fuel-tank-to-Frame" });
            CreateVariantOperationMappingInstance("Fifth_wheel", new List<string> { "Secure-Part-ID", "Attach-Fifth-Wheel-to-Frame" });
            CreateVariantOperationMappingInstance("Side_panel_red", new List<string> { "Secure-Part-ID", "Attach-Side-Panel" });
            CreateVariantOperationMappingInstance("Side_panel_yellow", new List<string> { "Secure-Part-ID", "Attach-Side-Panel" });
            CreateVariantOperationMappingInstance("Grey_side_cover", new List<string> { "Secure-Part-ID", "Attach-Side-Cover" });
            CreateVariantOperationMappingInstance("Heavy_horn", new List<string> { "Secure-Part-ID", "Place-and-Secure-Heavy-Horn-to-Cab" });
            CreateVariantOperationMappingInstance("Light_horn", new List<string> { "Secure-Part-ID", "Place-and-Secure-Light-Horn-to-Cab" });
            CreateVariantOperationMappingInstance("Heavy_engine", new List<string> { "Secure-Part-ID", "Setup-Heavy-Engine-Sound" });
            CreateVariantOperationMappingInstance("Light_engine", new List<string> { "Secure-Part-ID", "Setup-Light-Engine-Sound" });
            CreateVariantOperationMappingInstance("Interior_lights", new List<string> { "Secure-Part-ID", "Place-and-Secure-Interior-Lights-to-Cab" });
            CreateVariantOperationMappingInstance("Working_lights", new List<string> { "Secure-Part-ID", "Place-and-Secure-Working-Lights-to-Cab" });
            CreateVariantOperationMappingInstance("Beamer_lights", new List<string> { "Secure-Part-ID", "Place-and-Secure-Beamer-Lights-to-Cab" });
            CreateVariantOperationMappingInstance("Rotational_lights", new List<string> { "Secure-Part-ID", "Place-and-Secure-Rotational-Lights-to-Cab" });
            CreateVariantOperationMappingInstance("Front_lights", new List<string> { "Secure-Part-ID", "Place-and-Secure-Front-Lights-to-Cab" });
            CreateVariantOperationMappingInstance("Indicator_lights", new List<string> { "Secure-Part-ID", "Place-and-Secure-Indicator-Lights-to-Cab" });
        }

        //Old variant matrix
        public void createTestData7()
        {
            CreateOperationInstance("Load-AGV", "Load-AGV", null, null);
            CreateOperationInstance("Secure-Part-ID", "Secure-Part-ID", null, null);
            CreateOperationInstance("Place-and-attach-Steering-axle", "Place-and-attach-Steering-axle", new List<string> { "Secure-Part-ID" }, null);
            CreateOperationInstance("Place-and-attach-Driving-axle", "Place-and-attach-Driving-axle", new List<string> { "Secure-Part-ID" }, null);
            CreateOperationInstance("Place-and-attach-Tag-Push-axle", "Place-and-attach-Tag-Push-axle", new List<string> { "Secure-Part-ID" }, null);
            CreateOperationInstance("Drop-engine-and-Attach-to-Frame", "Drop-engine-and-Attach-to-Frame", new List<string> { "Secure-Part-ID" }, null);
            CreateOperationInstance("Pick-Baseplate", "Pick-Baseplate",null , null);
            CreateOperationInstance("Pick-2-Air-Tanks", "Pick-2-Air-Tanks", new List<string> { "Pick-Baseplate" }, null);
            CreateOperationInstance("Place-Air-Tank-1-on-Baseplate", "Place-Air-Tank-1-on-Baseplate", new List<string> { "Pick-2-Air-Tanks" }, null);
            CreateOperationInstance("Place-Air-Tank-2-on-Baseplate", "Place-Air-Tank-2-on-Baseplate", new List<string> { "Pick-2-Air-Tanks" }, null);
            CreateOperationInstance("Pick-Battrybox", "Pick-Battrybox", null, null);
            CreateOperationInstance("Place-Battrybox-on-Baseplate", "Place-Battrybox-on-Baseplate", new List<string> { "Pick-Battrybox" }, null);
            CreateOperationInstance("Pick-Urea-tank", "Pick-Urea-tank", null, null);
            CreateOperationInstance("Place-Urea-tank-on-Baseplate", "Place-Urea-tank-on-Baseplate", new List<string> { "Pick-Urea-tank" }, null);
            CreateOperationInstance("Attach-Accessory-Kit-to-Frame", "Attach-Accessory-Kit-to-Frame", new List<string> { "Secure-Part-ID" }, null);
            CreateOperationInstance("Attach-Fuel-tank-to-Frame", "Attach-Fuel-tank-to-Frame", new List<string> { "Secure-Part-ID" }, null);
            CreateOperationInstance("Attach-Side-Panel", "Attach-Side-Panel", new List<string> { "Secure-Part-ID" }, null);
            CreateOperationInstance("Attach-Side-Cover", "Attach-Side-Cover", new List<string> { "Secure-Part-ID" }, null);
            CreateOperationInstance("Attach-Fifth-Wheel-to-Frame", "Attach-Fifth-Wheel-to-Frame", new List<string> { "Secure-Part-ID" }, null);
            CreateOperationInstance("Attach-Wheels-to-Axles", "Attach-Wheels-to-Axles", new List<string> { "Secure-Part-ID" }, null);
            CreateOperationInstance("Place-and-Secure-Cab-to-Frame", "Place-and-Secure-Cab-to-Frame", new List<string> { "Secure-Part-ID" }, null);


            CreateVariantInstance("frame-rigid", 1, "frame-rigid", new List<string> { "Load-AGV" });
            CreateVariantInstance("frame-tractor", 2, "frame-tractor", new List<string> { "Load-AGV" });
            CreateVariantInstance("cab-version1", 3, "cab-version1", new List<string> { "Secure-Part-ID", "Place-and-Secure-Cab-to-Frame" });
            CreateVariantInstance("cab-version2", 4, "cab-version2", new List<string> { "Secure-Part-ID", "Place-and-Secure-Cab-to-Frame" });
            CreateVariantInstance("cab-version3", 5, "cab-version3", new List<string> { "Secure-Part-ID", "Place-and-Secure-Cab-to-Frame" });
            CreateVariantInstance("Steering-axle", 6, "Steering-axle", new List<string> { "Secure-Part-ID", "Place-and-attach-Steering-axle" });
            CreateVariantInstance("Driving-axle", 7, "Driving-axle", new List<string> { "Secure-Part-ID", "Place-and-attach-Driving-axle" });
            CreateVariantInstance("Tag-push-axle", 8, "Tag-push-axle", new List<string> { "Secure-Part-ID", "Place-and-attach-Tag-Push-axle" });
            CreateVariantInstance("Engine", 9, "Engine", new List<string> { "Secure-Part-ID", "Drop-engine-and-Attach-to-Frame" });
            CreateVariantInstance("Baseplate", 10, "Baseplate", new List<string> { "Pick-Baseplate" });
            CreateVariantInstance("Air-tanks", 11, "Air-tanks", new List<string> { "Pick-2-Air-Tanks", "Place-Air-Tank-1-on-Baseplate", "Place-Air-Tank-2-on-Baseplate" });
            CreateVariantInstance("Battry-box", 12, "Battry-box", new List<string> { "Pick-Battrybox", "Place-Battrybox-on-Baseplate" });
            CreateVariantInstance("Urea-tank", 13, "Urea-tank", new List<string> { "Pick-Urea-tank", "Place-Urea-tank-on-Baseplate" });
            CreateVariantInstance("Accessory-Kit", 14, "Accessory-Kit", new List<string> { "Secure-Part-ID", "Attach-Accessory-Kit-to-Frame" });
            CreateVariantInstance("Fuel-tank", 15, "Fuel-tank", new List<string> { "Secure-Part-ID", "Attach-Fuel-tank-to-Frame" });
            CreateVariantInstance("Side-panel-red", 16, "Side-panel-red", new List<string> { "Secure-Part-ID", "Attach-Side-Panel" });
            CreateVariantInstance("Side-panel-yellow", 17, "Side-panel-yellow", new List<string> { "Secure-Part-ID", "Attach-Side-Panel" });
            CreateVariantInstance("Grey-side-cover", 18, "Grey-side-cover", new List<string> { "Secure-Part-ID", "Attach-Side-Cover" });
            CreateVariantInstance("Fifth-wheel", 19, "Fifth-wheel", new List<string> { "Secure-Part-ID", "Attach-Fifth-Wheel-to-Frame" });
            CreateVariantInstance("Wheel", 20, "Wheel", new List<string> { "Secure-Part-ID", "Attach-Wheels-to-Axles" });

            CreateVariantGroupInstance("frame", "choose exactly one", new List<string> { "frame-rigid", "frame-tractor" });
            CreateVariantGroupInstance("cab", "choose exactly one", new List<string> { "cab-version1", "cab-version2", "cab-Version3" });
            CreateVariantGroupInstance("Axles", "choose at least one", new List<string> { "Steering-axle", "Driving-axle", "Tag-push-axle" });
            CreateVariantGroupInstance("Power-train", "choose exactly one", new List<string> { "Engine" });
            CreateVariantGroupInstance("Accessory-kit", "choose at least one", new List<string> { "Baseplate", "Air-tanks", "Battry-box", "Urea-tank" });
            CreateVariantGroupInstance("Fuel-tank", "choose exactly one", new List<string> { "Fuel-tank" });
            CreateVariantGroupInstance("Chassis-sides", "choose exactly one", new List<string> { "Side-panel-red", "Side-panel-yellow", "Grey-side-cover" });
            CreateVariantGroupInstance("Fifth-wheel", "choose exactly one", new List<string> { "Fifth-wheel" });
            CreateVariantGroupInstance("Wheels", "choose exactly one", new List<string> { "Wheel" });


            addConstraint("and frame-rigid cab-version2");

            CreateVariantOperationMappingInstance("frame-rigid", new List<string> { "Load-AGV" });
            CreateVariantOperationMappingInstance("frame-tractor", new List<string> { "Load-AGV" });
            CreateVariantOperationMappingInstance("cab-version1", new List<string> { "Secure-Part-ID", "Place-and-Secure-Cab-to-Frame" });
            CreateVariantOperationMappingInstance("cab-version2", new List<string> { "Secure-Part-ID", "Place-and-Secure-Cab-to-Frame" });
            CreateVariantOperationMappingInstance("cab-version3", new List<string> { "Secure-Part-ID", "Place-and-Secure-Cab-to-Frame" });
            CreateVariantOperationMappingInstance("Steering-axle", new List<string> { "Secure-Part-ID", "Place-and-attach-Steering-axle" });
            CreateVariantOperationMappingInstance("Driving-axle", new List<string> { "Secure-Part-ID", "Place-and-attach-Driving-axle" });
            CreateVariantOperationMappingInstance("Tag-push-axle", new List<string> { "Secure-Part-ID", "Place-and-attach-Tag-Push-axle" });
            CreateVariantOperationMappingInstance("Engine", new List<string> { "Secure-Part-ID", "Drop-engine-and-Attach-to-Frame" });
            CreateVariantOperationMappingInstance("Baseplate", new List<string> { "Pick-Baseplate" });
            CreateVariantOperationMappingInstance("Air-tanks", new List<string> { "Pick-2-Air-Tanks", "Place-Air-Tank-1-on-Baseplate", "Place-Air-Tank-2-on-Baseplate" });
            CreateVariantOperationMappingInstance("Battry-box", new List<string> { "Pick-Battrybox", "Place-Battrybox-on-Baseplate" });
            CreateVariantOperationMappingInstance("Urea-tank", new List<string> { "Pick-Urea-tank", "Place-Urea-tank-on-Baseplate" });
            CreateVariantOperationMappingInstance("Accessory-Kit", new List<string> { "Secure-Part-ID", "Attach-Accessory-Kit-to-Frame" });
            CreateVariantOperationMappingInstance("Fuel-tank", new List<string> { "Secure-Part-ID", "Attach-Fuel-tank-to-Frame" });
            CreateVariantOperationMappingInstance("Side-panel-red", new List<string> { "Secure-Part-ID", "Attach-Side-Panel" });
            CreateVariantOperationMappingInstance("Side-panel-yellow", new List<string> { "Secure-Part-ID", "Attach-Side-Panel" });
            CreateVariantOperationMappingInstance("Grey-side-cover", new List<string> { "Secure-Part-ID", "Attach-Side-Cover" });
            CreateVariantOperationMappingInstance("Fifth-wheel", new List<string> { "Secure-Part-ID", "Attach-Fifth-Wheel-to-Frame" });
            CreateVariantOperationMappingInstance("Wheel", new List<string> { "Secure-Part-ID", "Attach-Wheels-to-Axles" });
        }*/

    }
}
