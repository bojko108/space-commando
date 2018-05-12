using System.Collections;
using System.Collections.Generic;
using Import;
using UnityEngine;

namespace Import
{
    /// <summary>
    /// 
    /// </summary>
    public enum EnumGeometryType
    {
        /// <summary>
        /// 
        /// </summary>
        Point,
        Polygon,
        Line
    }

    public class MapFeatureGeometry
    {
        /// <summary>
        /// 
        /// </summary>
        public List<Vertex> Vertices;

        public EnumGeometryType Type { get { return this.geometryType; } }
        public bool IsEmpty { get { return this.Vertices.Count < 1; } }

        /// <summary>
        /// 
        /// </summary>
        private EnumGeometryType geometryType;

        public MapFeatureGeometry(EnumGeometryType type)
        {
            this.geometryType = type;
            this.Vertices = new List<Vertex>();
        }

        public MapFeatureGeometry(EnumGeometryType type, List<Vertex> vertices)
        {
            this.geometryType = type;
            this.Vertices = vertices;
        }

        public Vector3 GetCentroid()
        {
            Vector3 total = Vector3.zero;

            // no vertices so return 0 vector
            if (this.IsEmpty) return total;

            foreach (Vertex vertex in this.Vertices)
            {
                total += vertex.ToVector3();
            }

            return total / this.Vertices.Count;
        }

        /// <summary>
        /// Triangulate geometry vertices to create a mesh
        /// </summary>
        /// <returns>triangulated mesh</returns>        
        public Poly2Mesh.Polygon ToPolygonMesh()
        {
            Poly2Mesh.Polygon polygon = new Poly2Mesh.Polygon();
            polygon.outside = this.Vertices.ConvertAll((v) => v.ReduceToVector3(this.GetCentroid()));
            return polygon;
        }

        public void ProjectToGeographic()
        {

        }

        public void ProjectToWebMercator()
        {
            this.Vertices.ForEach(v => v = v.ProjectToWebMercator());
        }

        public void SetScale(float scale)
        {
            this.Vertices.ForEach(v => v = v.Scale(scale));
        }
    }
}
