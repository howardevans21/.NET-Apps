using DWiPipeline.DataModel;
using DWPipelineWebService.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace DWiPipeline
{
    internal class iPipeLineXMLReader
    {
        public iPipeLineXMLReader() { }

        public void Read()
        {
                XmlDocument xmldoc = new XmlDocument();
              //  xmldoc.Load(fileName);

                foreach (XmlNode node in xmldoc.ChildNodes)
                {
                    if (node.Name.ToLower() == "txlife")
                    {
                        foreach (XmlNode childNode in node.ChildNodes)
                        {
                            if (childNode.Name.ToLower() == "txliferequest")
                            {
                                foreach (XmlNode childNode2 in childNode.ChildNodes)
                                {
                                    if (childNode2.Name.ToLower() == "olife")
                                    {

                                        /***********************************
                                         * Create Policy Object
                                         **********************************/
                                        PolicyModel pm = new PolicyModel("1");
                                        pm.attachments = new List<AttachmentModel>();

                                        foreach (XmlNode childNode3 in childNode2.ChildNodes)
                                        {
                                            /***********************************************************************
                                             * START Account Holding
                                             **********************************************************************/
                                            if (childNode3.Name.ToLower() == "holding")
                                            {
                                                foreach (XmlNode childNode4 in childNode3.ChildNodes)
                                                {
                                                    if (childNode4.Name.ToLower() == "policy")
                                                    {

                                                        foreach (XmlNode childNode5 in childNode4.ChildNodes)
                                                        {
                                                            if (childNode5.Name.ToLower() == "life")
                                                            {
                                                                foreach (XmlNode childNode6 in childNode5.ChildNodes)
                                                                {
                                                                    if (childNode6.Name.ToLower() == "coverage")
                                                                    {

                                                                    }
                                                                }
                                                            }

                                                            if(childNode5.Name.ToLower() == "applicationinfo")
                                                            {
                                                                foreach(XmlNode childNode7 in childNode5.ChildNodes)
                                                                {
                                                                    if(childNode7.Name.ToLower() == "olifeextension")
                                                                    {

                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }

                                                   
                                                }  /***********************************************************************
                                                    * END Account Holding
                                             **********************************************************************/
                                            }

                                            /***********************************************************************
                                                 * START FORM INSTANCE 
                                              
                                             **********************************************************************/
                                            if (childNode3.Name.ToLower() == "forminstance")
                                            {
                                                foreach (XmlNode childNode7 in childNode3.ChildNodes)
                                                {
                                                    string attachmentBasicType = "";
                                                    string attachmentData = "";
                                              //      string attachmentDescription = "";
                                                    if (childNode7.Name.ToLower() == "attachment")
                                                    {
                                                        foreach (XmlNode childNode8 in childNode7.ChildNodes)
                                                        {
                                                        
                                                            if (childNode8.Name.ToLower() == "attachmentbasictype")
                                                                attachmentBasicType = childNode8.InnerXml;

                                                            if (childNode8.Name.ToLower() == "attachmentdata")
                                                                attachmentData = childNode8.InnerXml; 

                                                            if(attachmentData.Length > 0)
                                                            {
                                                                Utils util = new Utils();
                                                                //util.ConvertHexToFile(attachmentData);
                                                            }
                                                        }
                                                    }
                                                }

                                            }
                                        }

                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }

