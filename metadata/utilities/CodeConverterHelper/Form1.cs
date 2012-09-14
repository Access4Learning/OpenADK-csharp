using System;
using System.Text;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Xml;

namespace WindowsApplication3
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class Form1 : System.Windows.Forms.Form
	{
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.TextBox textBox2;
		private System.Windows.Forms.TextBox textBox3;
		private System.Windows.Forms.Label label1;
        private CheckBox chkXml;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public Form1()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.chkXml = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(40, 24);
            this.textBox1.MaxLength = 65535999;
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(600, 328);
            this.textBox1.TabIndex = 0;
            this.textBox1.Text = "textBox1";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(416, 488);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(160, 32);
            this.button1.TabIndex = 1;
            this.button1.Text = "button1";
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(48, 416);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(120, 20);
            this.textBox2.TabIndex = 2;
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(208, 416);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(376, 20);
            this.textBox3.TabIndex = 3;
            this.textBox3.Text = "textBox3";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(40, 392);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(144, 24);
            this.label1.TabIndex = 4;
            this.label1.Text = "codeset";
            // 
            // chkXml
            // 
            this.chkXml.AutoSize = true;
            this.chkXml.Location = new System.Drawing.Point(42, 457);
            this.chkXml.Name = "chkXml";
            this.chkXml.Size = new System.Drawing.Size(48, 17);
            this.chkXml.TabIndex = 5;
            this.chkXml.Text = "XML";
            this.chkXml.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(688, 542);
            this.Controls.Add(this.chkXml);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new Form1());
		}

		private void button1_Click(object sender, System.EventArgs e)
		{
            if (chkXml.Checked)
            {
                OnFixupXmlEnumList();
            }
            else
            {
                OnConvertRawCodeList();
            }
		}

        private void OnFixupXmlEnumList()
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(textBox1.Text);
            foreach (XmlElement child in doc.DocumentElement.ChildNodes)
            {
                child.RemoveAttribute( "name" );
                XmlAttribute name = doc.CreateAttribute( "name" );
                name.Value = makeName(child.GetAttribute("desc"));
                child.Attributes.InsertBefore(name, child.Attributes[0]);
            }

            textBox1.Text = doc.OuterXml;
        }

        private void OnConvertRawCodeList()
        {
            StringBuilder builder = new StringBuilder();
            string[] data = textBox1.Lines;
            string codeset = textBox2.Text.Trim();
            if (codeset.Length > 0)
            {
                codeset += "_";
            }
            foreach (string line in data)
            {
                string[] parts = line.Split(new char[] { (char)9 }, 2);
                if (parts.Length == 2)
                {
                    builder.Append("<value name=");
                    builder.Append('"');
                    builder.Append(codeset);
                    builder.Append(makeName(parts[1]));
                    builder.Append('"');
                    builder.Append(" value=");
                    builder.Append('"');
                    builder.Append(xmlEncode(parts[0]));
                    builder.Append('"');
                    builder.Append(" desc=");
                    builder.Append('"');
                    builder.Append(xmlEncode(parts[1]));
                    builder.Append('"');
                    builder.Append(" />");
                    builder.Append("\r\n");
                }
                //<value value="0512" desc="Achievement level" name="ACHIEVEMENT_LEVEL"/>
            }

            textBox1.Text = builder.ToString();

        }

        private String xmlEncode(String data)
        {
            data = data.Replace("&", "&amp;");
            data = data.Replace("\"", "&quot;");
            return data;
        }

		private string makeName( string str )
		{
            str = str.Replace("&", "");
            str = str.Replace("\"", "");
			if( str.IndexOf( "—" ) > 0 )
			{
				str = str.Substring( 0, str.IndexOf( "—" ) );
			}
			if( str.Length > 30 )
			{
				str = str.Substring( 0, 30 );
			}

			str = str.Replace( "'", "" ) ;
			str = str.Replace( ",", "" ) ;
			str = str.Replace( '/', '_' ) ;
			str = str.Replace( '-', '_' ) ;
            str = str.Replace( '(', ' ' ) ;
			str = str.Replace( ')', ' ' ) ;
			str = str.Replace( '\\', '_' ) ;
            str = str.Replace("  ", " ");
			str = str.Replace( ' ', '_' ).ToUpper() ;

			int len = str.LastIndexOf( "_" );
			if( len >= 20 )
			{
				str = str.Substring( 0, len  );
			}
			while( str.EndsWith( "_" ) )
			{
				str = str.Substring( 0, str.Length -1 );
			}

			return str;
		}
	}
}
