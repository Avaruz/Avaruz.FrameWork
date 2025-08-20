/**********************************
 * Title:       Description Attribute Called from a Referenced Property's Description
 * Author:      Juergen Thomas
 * Email:       post@vs-polis.de
 * Environment: WinXP, NET 2.0
 * Keywords:    ReferencedDescriptionAttribute, Reference, DescriptionAttribute,
 *              Description, Attribute
 * Description: An article how to show a description attribute referencing
 *              the description attribute of another class and a specific property
 * Section      General Programming
 * SubSection   Programming Tips - General
 *
 * Example
 * -------
 * [ReferencedDescription(typeof(System.Windows.Forms.MaskedTextBox), "Mask")]
 *
 * This attribute calls the description of Mask property in MaskedTextBox class.
**********************************/

#region Usings
using System;
using System.ComponentModel;
#endregion

namespace Avaruz.FrameWork.Controls.Win
{

    /// <summary>
    /// ReferencedDescriptionAttribute shows the description of a specific property
    /// in an existing class (the "referenced type").
    /// </summary>
    public class ReferencedDescriptionAttribute : DescriptionAttribute
    {

        public ReferencedDescriptionAttribute(Type referencedType, string propertyName)
        {
            //	default description
            string result = "Referenced description not available";

            //	gets the properties of the referenced type
            PropertyDescriptorCollection properties
                = TypeDescriptor.GetProperties(referencedType);

            if (properties != null)
            {

                // gets a PropertyDescriptor to the specific property.
                PropertyDescriptor property = properties[propertyName];
                if (property != null)
                {
                    //  gets the attributes of the required property
                    AttributeCollection attributes = property.Attributes;

                    // Gets the description attribute from the collection.
                    DescriptionAttribute descript
                        = (DescriptionAttribute)attributes[typeof(DescriptionAttribute)];

                    // register the referenced description
                    if (!String.IsNullOrEmpty(descript.Description))
                    {
                        result = descript.Description;
                    }
                }

            }
            DescriptionValue = result;

        }

    }

}
