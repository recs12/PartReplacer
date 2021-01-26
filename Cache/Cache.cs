using System;
using System.Runtime.InteropServices;

namespace PartReplacer
{

    public class Cache
    {
        public static string GetJde(string partCachePath)
        {
            SolidEdgeFileProperties.PropertySets propertySets = null;
            SolidEdgeFileProperties.Properties properties = null;
            SolidEdgeFileProperties.Property property = null;

            try
            {
                // Create new instance of the PropertySets object
                propertySets = new SolidEdgeFileProperties.PropertySets();

                // Open a file
                propertySets.Open(partCachePath, true);
                // Get a reference to the SummaryInformation properties
                properties = (SolidEdgeFileProperties.Properties)propertySets["Custom"];

                // Get a reference to the Title property by name
                property = (SolidEdgeFileProperties.Property)properties["JDELITM"];



                // TODO: Print the list of available option.
                return property.Value.ToString();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
            finally
            {
                if (property != null)
                {
                    Marshal.ReleaseComObject(property);
                }
                if (properties != null)
                {
                    Marshal.ReleaseComObject(properties);
                }
                if (propertySets != null)
                {
                    propertySets.Close();
                    Marshal.ReleaseComObject(propertySets);
                }
            }
        }
    }
}
