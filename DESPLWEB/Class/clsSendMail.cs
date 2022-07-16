using System;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.IO;

namespace DESPLWEB
{
    public class clsSendMail
    {
        //string cnStr = System.Configuration.ConfigurationSettings.AppSettings["conStr"].ToString();
        string cnStr = System.Configuration.ConfigurationManager.AppSettings["conStr"].ToString();

        public void SendMail(string mTo, string mCc, string mSubject, string mBody, string mAttachment, string mReplyTo)
        {
            try
            {
                if (mBody.Trim() == "" || mTo.Contains("na@")
                    || mTo.Contains("NA@") || mTo.Contains("unknown"))
                {
                    return;
                }

                string mFrom = "";
                string mPass = "";

                mTo = mTo.Replace("/", ",");
                mTo = mTo.Replace("'", "");
                mTo = mTo.Replace(";", ",");

                mTo = mTo.TrimEnd(',');
                mTo = mTo.TrimEnd(';');
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)48
                    | (SecurityProtocolType)192 |
                (SecurityProtocolType)768 | (SecurityProtocolType)3072;
                
                    LabDataDataContext dc = new LabDataDataContext();
                    var master = dc.MasterSetting_View(0);

                    foreach (var mst in master)
                    {
                        mFrom = mst.MASTER_MailFromId_var;
                        mPass = mst.MASTER_MailFromPassword_var;

                    }
                mCc = mCc.TrimEnd(',');
                mCc = mCc.TrimEnd(';');
                //if (mCc != "")
                //    mCc += "," + mFrom;
                //else
                //    mCc = mFrom;

                mCc = mCc.TrimEnd(',');
                mCc = mCc.TrimEnd(';');

                MailMessage msg = new MailMessage();
                msg.From = new MailAddress(mFrom);
                msg.To.Add(mTo);
                //if (mReplyTo != "")
                //    msg.ReplyTo = new MailAddress(mReplyTo);
                if (mCc != "")
                    msg.CC.Add(mCc);
                //msg.Bcc.Add("cat@cqra.com");
                if (mAttachment != "")
                {
                    if (mAttachment.Contains(",") == true)
                    {
                        string[] attch = mAttachment.Split(',');
                        for (int x = 0; x < attch.Count(); x++)
                        {
                            msg.Attachments.Add(new Attachment(attch[x]));
                        }
                    }
                    else
                    {
                        msg.Attachments.Add(new Attachment(@mAttachment));
                    }
                }
                msg.Subject = mSubject;
                msg.IsBodyHtml = true;
                msg.Body = mBody;

                string smtpDtl;
                if (mFrom.Contains("gmail.com"))
                    smtpDtl = "smtp.gmail.com";
                else   //if (mFrom.Contains("durocrete.acts-int.com"))
                    smtpDtl = "smtp.office365.com";


                SmtpClient smt = new SmtpClient(smtpDtl);
                smt.UseDefaultCredentials = false;
                if (mFrom.Contains("gmail.com") || mFrom.Contains("durocrete.acts-int.com") || mFrom.Contains("onmicrosoft.com"))
                    smt.EnableSsl = true;

                smt.Port = 587;

                smt.Credentials = new NetworkCredential(mFrom, mPass);
                smt.Send(msg);

                if (mAttachment != "")
                    msg.Attachments.Dispose();
                msg.Dispose();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public void SendMailProposal(string mTo, string mCc, string mSubject, string mBody, string mAttachment, string mReplyTo)
        {
            try
            {
                //string mFrom = "reports.pune@durocrete.com";
                //string mPass = "reportspune1";
                //string mFrom = "duroreports.pune@gmail.com";
                //string mPass = "Durocretepune";
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)48
                    | (SecurityProtocolType)192 |
                (SecurityProtocolType)768 | (SecurityProtocolType)3072;
                mTo = mTo.Replace("/", ",");
                mTo = mTo.Replace("'", "");
                mTo = mTo.Replace(";", ",");

                mTo = mTo.TrimEnd(',');
                mTo = mTo.TrimEnd(';');
                string mFrom = "";
                string mPass = "";
               
                LabDataDataContext dc = new LabDataDataContext();
                var master = dc.MasterSetting_View(0);
                foreach (var mst in master)
                {
                    mFrom = mst.MASTER_ProposalMailFromId_var;
                    mPass = mst.MASTER_ProposalMailFromPassword_var;
                }
                mTo = mTo.Replace("/", ",");
                mTo = mTo.Replace("'", "");
                mTo = mTo.Replace(";", ",");
                mTo = mTo.TrimEnd(',');
                mTo = mTo.TrimEnd(';');
                if (mCc != "")
                    mCc += "," + mFrom;
                else
                    mCc = mFrom;

                MailMessage msg = new MailMessage();
                msg.From = new MailAddress(mFrom);
                msg.To.Add(mTo);
                //if (mReplyTo != "")
                //    msg.ReplyTo = new MailAddress(mReplyTo);
                if (mCc != "")
                    msg.CC.Add(mCc);


                //msg.Bcc.Add("cat@cqra.com");
                if (mAttachment != "")
                {
                    if (mAttachment.Contains(",") == true)
                    {
                        string[] attch = mAttachment.Split(',');
                        for (int x = 0; x < attch.Count(); x++)
                        {
                            msg.Attachments.Add(new Attachment(attch[x]));
                        }
                    }
                    else
                    {
                        msg.Attachments.Add(new Attachment(@mAttachment));
                    }
                }
                msg.Subject = mSubject;
                msg.IsBodyHtml = true;
                msg.Body = mBody;

                string smtpDtl;
                if (mFrom.Contains("gmail.com"))
                    smtpDtl = "smtp.gmail.com";
                else if (mFrom.Contains("durocrete.acts-int.com"))
                    smtpDtl = "smtp.office365.com";
                else
                    smtpDtl = "email.cqra.com";
                SmtpClient smt = new SmtpClient(smtpDtl);
                if (mFrom.Contains("gmail.com") || mFrom.Contains("durocrete.acts-int.com"))
                    smt.EnableSsl = true;
                smt.Port = 587;
                smt.Credentials = new NetworkCredential(mFrom, mPass);
                smt.Send(msg);

                if (mAttachment != "")
                    msg.Attachments.Dispose();
                msg.Dispose();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public void sendSMS(string ToNumber, string msg, string sender, string templateID)
        {
            

            string result = "";
            //return;
            WebRequest request = null;
            HttpWebResponse response = null;
            try
            {
                String userid = "cqraa";
                String passwd = "123123";

                string strPEID = "1001708803183708129";
                //String url = "http://www.smswave.in/panel/sendsms.php?sender=" + sender + "&PhoneNumber=" + ToNumber +
                //"&Text="+ msg + "&user=" + userid + "&password=" + passwd;

                String url = "http://www.smswave.in/panel/sendsms2021.php?user=" + userid + "&password=" + passwd +
                    "&sender=" + sender + "&PhoneNumber=" + ToNumber + "&Text=" + msg +
                    "&msgType=PT & pe_id=" + strPEID + "&template_id=" + templateID;
                request = WebRequest.Create(url);
                response = (HttpWebResponse)request.GetResponse();
                Stream stream = response.GetResponseStream();
                Encoding ec = System.Text.Encoding.GetEncoding("utf-8");
                StreamReader reader = new System.IO.StreamReader(stream, ec);
                result = reader.ReadToEnd();
                Console.WriteLine(result);
                reader.Close();
                stream.Close();
            }
            catch (Exception exp)
            {
                //                Console.WriteLine(exp.ToString());
                string s = exp.Message;

            }
            finally
            {
                if (response != null)
                    response.Close();
            }

        }

        public void sendSMS_old(string ToNumber, string msg, string sender)
        {
            string result = "";
            WebRequest request = null;
            HttpWebResponse response = null;
            try
            {
                //String sendToPhoneNumber = "9763804804";

                String userid = "cqra";
                String passwd = "123123";
                //String sender = "CQRAWF";
                String url = "http://www.smswave.in/panel/sendsms.php?sender=" + sender + "&PhoneNumber=" + ToNumber +
                "&Text=" + msg + "&user=" + userid + "&password=" + passwd;
                request = WebRequest.Create(url);
                //in case u work behind proxy, uncomment the commented code and provide correct details SMSWave API
                /*WebProxy proxy = new WebProxy("http://proxy:80/", true);
                proxy.Credentials = new NetworkCredential("userId", "password", "Domain");
                request.Proxy = proxy;
                */
                // Send the 'HttpWebRequest' and wait for response.
                response = (HttpWebResponse)request.GetResponse();
                Stream stream = response.GetResponseStream();
                Encoding ec = System.Text.Encoding.GetEncoding("utf-8");
                StreamReader reader = new System.IO.StreamReader(stream, ec);
                result = reader.ReadToEnd();
                Console.WriteLine(result);
                reader.Close();
                stream.Close();
            }
            catch (Exception exp)
            {
                //                Console.WriteLine(exp.ToString());
                string s = exp.Message;
            }
            finally
            {
                if (response != null)
                    response.Close();
            }

        }


   
    }
}
