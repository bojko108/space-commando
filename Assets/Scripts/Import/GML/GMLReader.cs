using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Xml;

namespace Import.GML
{
    /// <summary>
    /// Class for reading map data in GML format
    /// </summary>
    public class GMLReader
    {
        /// <summary>
        /// Map data bounds
        /// </summary>
        public MapBounds Bounds { get; private set; }
        /// <summary>
        /// List of features
        /// </summary>
        public List<MapFeature> Features { get; private set; }
        /// <summary>
        /// GML namespaces
        /// </summary>
        public XmlNamespaceManager GMLNamespaces { get; private set; }

        /// <summary>
        /// Read GML data
        /// </summary>
        /// <param name="gmlText">GML data as string</param>
        public GMLReader(string rootElementName, string gmlText)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(gmlText);

            this.GMLNamespaces = new XmlNamespaceManager(xmlDocument.NameTable);
            this.GMLNamespaces.AddNamespace("ogr", "http://ogr.maptools.org/");
            this.GMLNamespaces.AddNamespace("gml", "http://www.opengis.net/gml/3.2");

            this.parseGMLDocument(rootElementName, xmlDocument.DocumentElement);
        }

        /// <summary>
        /// Reads GML data
        /// </summary>
        /// <param name="rootElement">Root document element of the GML file</param>
        private void parseGMLDocument(string rootElementName, XmlElement rootElement)
        {
            try
            {
                #region READ FEATURES BOUNDS

                XmlNode bounds = rootElement.SelectSingleNode("/ogr:FeatureCollection/gml:boundedBy/gml:Envelope", GMLNamespaces);

                string lowerLeftCoordinates = bounds.SelectSingleNode("gml:lowerCorner", GMLNamespaces).ChildNodes[0].Value;
                string upperRightCoordinates = bounds.SelectSingleNode("gml:upperCorner", GMLNamespaces).ChildNodes[0].Value;

                this.Bounds = new MapBounds(new Vertex(lowerLeftCoordinates), new Vertex(upperRightCoordinates));

                #endregion

                #region READ FEATURES

                this.Features = new List<MapFeature>();

                XmlNodeList featureNodes = rootElement.SelectNodes("/ogr:FeatureCollection/ogr:featureMember/ogr:" + rootElementName, GMLNamespaces);

                foreach (XmlNode featureNode in featureNodes)
                {
                    string fid = featureNode.SelectSingleNode("ogr:osm_id", GMLNamespaces).ChildNodes[0].Value;

                    int dimension = this.GetAttribute<int>("srsDimension", featureNode.SelectSingleNode("ogr:geometryProperty/gml:Polygon/gml:exterior/gml:LinearRing/gml:posList", GMLNamespaces).Attributes);
                    string geometry = featureNode.SelectSingleNode("ogr:geometryProperty/gml:Polygon/gml:exterior/gml:LinearRing/gml:posList", GMLNamespaces).ChildNodes[0].Value;

                    List<Vertex> verticesList = new List<Vertex>();
                    string[] vertices = geometry.Split(new char[] { ' ' });

                    switch (dimension)
                    {
                        // X Y Z
                        case 3:
                            {
                                for (int i = 0; i < vertices.Length - 3; i = i + 3)
                                {
                                    verticesList.Add(new Vertex(String.Format("{0} {1} {2}", vertices[i], vertices[i + 1], vertices[i + 2])));
                                }

                                break;
                            }
                        // X Y
                        case 2:
                            {
                                for (int i = 0; i < vertices.Length - 2; i = i + 2)
                                {
                                    verticesList.Add(new Vertex(String.Format("{0} {1}", vertices[i], vertices[i + 1])));
                                }

                                break;
                            }
                        default: break;
                    }

                    MapFeature feature = new MapFeature(fid);
                    feature.SetGeometry(EnumGeometryType.Polygon, verticesList);

                    this.Features.Add(feature);
                }

                #endregion
            }
            catch
            {

            }
        }

        /// <summary>
        /// Gets an attribute value from an XML node
        /// </summary>
        /// <typeparam name="T">Return type</typeparam>
        /// <param name="attributeName">Attribute name</param>
        /// <param name="attributes">List of attributes</param>
        /// <returns>Attribute value, converted to the specified type</returns>
        T GetAttribute<T>(string attributeName, XmlAttributeCollection attributes)
        {
            string value = attributes[attributeName].Value;
            return (T)Convert.ChangeType(value, typeof(T));
        }
    }
}
