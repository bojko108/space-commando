using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Import
{
    /// <summary>
    /// Class for managing map features
    /// </summary>
    public class MapFeature
    {
        /// <summary>
        /// Feature ID
        /// </summary>
        public string FID { get; private set; }
        /// <summary>
        /// Feature layer name
        /// </summary>
        public string Layer { get; set; }
        /// <summary>
        /// Feature tag name
        /// </summary>
        public string Tag { get; set; }
        /// <summary>
        /// Feature attributes
        /// </summary>
        public MapFeatureAttributes Attributes { get; set; }
        /// <summary>
        /// Feature geometry
        /// </summary>
        public MapFeatureGeometry Geometry { get; set; }


        public MapFeature(string fid)
        {
            this.FID = fid;
            this.Attributes = new MapFeatureAttributes(new Dictionary<string, string>());
            this.Geometry = new MapFeatureGeometry(EnumGeometryType.Polygon);
        }

        public MapFeature(string fid, MapFeatureGeometry geometry)
        {
            this.FID = fid;
            this.Attributes = new MapFeatureAttributes(new Dictionary<string, string>());
            this.Geometry = geometry;
        }

        public MapFeature(string fid, MapFeatureAttributes attributes, MapFeatureGeometry geometry)
        {
            this.FID = fid;
            this.Attributes = attributes;
            this.Geometry = geometry;
        }

        public void SetAttributes(Dictionary<string,string> attributes)
        {
            this.Attributes.Add((attributes));
        }

        public void SetGeometry(EnumGeometryType type, List<Vertex> vertices)
        {
            this.Geometry = new MapFeatureGeometry(type, vertices);
        }

        public GameObject ToGameObject()
        {
            Poly2Mesh.Polygon polygon = this.Geometry.ToPolygonMesh();
            GameObject go = Poly2Mesh.CreateGameObject(polygon, this.FID);
            return go;
        }
    }
}