using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Import.GeoJSON
{

    [System.Serializable]
    public class FeatureCollection : GeoJSONObject
    {

        public MapBounds bbox;

        public List<FeatureObject> features;

        public FeatureCollection(string encodedString)
        {
            features = new List<FeatureObject>();
            
            JSONObject jsonObject = new JSONObject(encodedString);

            bbox = new MapBounds();
            ParseBBOX(jsonObject["bbox"]);

            ParseFeatures(jsonObject["features"]);
            type = "FeatureCollection";
        }

        public FeatureCollection(JSONObject jsonObject) : base(jsonObject)
        {
            features = new List<FeatureObject>();
            ParseFeatures(jsonObject["features"]);

            bbox = new MapBounds();
            ParseBBOX(jsonObject["bbox"]);
        }

        public FeatureCollection()
        {
            features = new List<FeatureObject>();
            type = "FeatureCollection";
        }

        protected void ParseFeatures(JSONObject jsonObject)
        {
            foreach (JSONObject featureObject in jsonObject.list)
            {
                features.Add(new FeatureObject(featureObject));
            }
        }

        protected void ParseBBOX(JSONObject jsonObject)
        {
            List<float> bounds = new List<float>();

            foreach (JSONObject bboxObject in jsonObject.list)
            {
                bounds.Add(bboxObject.f);
            }

            // if no elevation data
            if (bounds.Count == 4)
            {
                Vertex lowerLeft = new Vertex(bounds[1], bounds[0], 0);
                Vertex upperRight = new Vertex(bounds[3], bounds[2], 0);

                bbox.LowerLeft = lowerLeft;
                bbox.UpperRight = upperRight;
            }

            if (bounds.Count == 6)
            {
                Vertex lowerLeft = new Vertex(bounds[1], bounds[0], bounds[2]);
                Vertex upperRight = new Vertex(bounds[4], bounds[3], bounds[5]);

                bbox.LowerLeft = lowerLeft;
                bbox.UpperRight = upperRight;
            }
        }

        override protected void SerializeContent(JSONObject rootObject)
        {

            JSONObject jsonFeatures = new JSONObject(JSONObject.Type.ARRAY);
            foreach (FeatureObject feature in features)
            {
                jsonFeatures.Add(feature.Serialize());
            }
            rootObject.AddField("features", jsonFeatures);
        }

    }
}
