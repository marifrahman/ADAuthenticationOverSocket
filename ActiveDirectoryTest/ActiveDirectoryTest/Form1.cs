using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;

using System.Runtime.InteropServices; 

namespace ActiveDirectoryTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        //        FYI, PrincipalContext implements IDisposable, so better remember to wrap this in a using clause. – Jeremy McGee Jul 22 '10 at 10:51
        //12	

        //In my domain, I had to specify pc.ValidateCredentials("myuser", "mypassword", ContextOptions.Negotiate) or I would get System.DirectoryServices.Protocols.DirectoryOperationException: The server cannot handle directory requests. – Alex Peck Jun 29 '11 at 14:14
        //3	

        //If a password's expired or the accounts disabled, then ValidateCredentials will return false. Unfortuantly, it doesn't tell you why it's returned false (which is a pity as it means I can't do something sensible like redirect the user to change their password). – Chris J Sep 8 '11 at 15:10
        //22	upvote
        //    flag
        //Also beware the 'Guest' account -- if the domain-level Guest account is enabled, ValidateCredentials returns true if you give it a non-existant user. As a result, you may want to call UserPrinciple.FindByIdentity to see if the passed in user ID exists first. 



        private void button1_Click(object sender, EventArgs e)
        {
            //DirectoryEntry adsEntry = new DirectoryEntry("LDAP://DC=ad.local", txtUserName.Text, txtPwd.Text);
            //DirectorySearcher adsSearcher = new DirectorySearcher(adsEntry);
            ////adsSearcher.Filter = "(&(objectClass=user)(objectCategory=person))";
            //adsSearcher.Filter = "(sAMAccountName=" + txtUserName.Text + ")";

            //try
            //{
            //    SearchResult adsSearchResult = adsSearcher.FindOne();
            //    //bSucceeded = true;

            //    //strAuthenticatedBy = "Active Directory";
            //    //strError = "User has been authenticated by Active Directory.";
            //    label1.Text = "User has been authenticated by Active Directory.";

            //}
            //catch (Exception ex)
            //{
            //    // Failed to authenticate. Most likely it is caused by unknown user
            //    // id or bad strPassword.
            //    label1.Text = ex.Message;

            //}
            //finally
            //{
            //    adsEntry.Close();
            //}

            using (PrincipalContext pc = new PrincipalContext(ContextType.Domain, "ad.local"))
            {
                // validate the credentials
                //pc.UserPrinciple.FindByIdentity
                UserPrincipal userprincipal = UserPrincipal.FindByIdentity(pc, txtUserName.Text);
                if (userprincipal != null)
                {
                    bool isValid = pc.ValidateCredentials(txtUserName.Text, txtPwd.Text);

                    if (isValid)
                        textBox1.Text = userprincipal.Sid.ToString();
                    else
                        textBox1.Text = "Not Authenticated";


                }
                else
                {
                    textBox1.Text = "Din find the user";
                }


            }


        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (var context = new PrincipalContext(ContextType.Domain, "ad.local"))
            {
                using (var searcher = new PrincipalSearcher(new UserPrincipal(context)))
                {
                    foreach (var result in searcher.FindAll())
                    {
                        DirectoryEntry de = result.GetUnderlyingObject() as DirectoryEntry;
                        //Console.WriteLine("First Name: " + de.Properties["givenName"].Value);
                        //Console.WriteLine("Last Name : " + de.Properties["sn"].Value);
                        //Console.WriteLine("SAM account name   : " + de.Properties["samAccountName"].Value);
                        //Console.WriteLine("User principal name: " + de.Properties["userPrincipalName"].Value);
                        //Console.WriteLine();

                        textBox2.Text += de.Properties["samAccountName"].Value + "," ;

                        byte[] SID = (byte[])de.Properties["objectSid"][0];
                        textBox2.Text += GetSidString(SID) + Environment.NewLine;

                    }
                }
            }
            

        }

        [DllImport("advapi32", CharSet = CharSet.Auto, SetLastError = true)]
        static extern bool ConvertSidToStringSid([MarshalAs(UnmanagedType.LPArray)] byte[] pSID, out IntPtr ptrSid);

        public static string GetSidString(byte[] sid)
        {
            IntPtr ptrSid;
            string sidString;
            if (!ConvertSidToStringSid(sid, out ptrSid))
                throw new System.ComponentModel.Win32Exception();
            try
            {
                sidString = Marshal.PtrToStringAuto(ptrSid);
            }
            finally
            {
                Marshal.FreeHGlobal(ptrSid);
            }
            return sidString;
        }


    }
}
