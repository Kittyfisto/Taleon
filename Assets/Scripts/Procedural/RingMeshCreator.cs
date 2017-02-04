using UnityEngine;

namespace Assets.Scripts.Procedural
{
	[RequireComponent(typeof(MeshFilter))]
	public class RingMeshCreator
		: MonoBehaviour
	{
		public float InnerRadius = 1;
		public float OuterRadius = 2;
		public int NumSegments = 10;

		private int _numSegments;
		private float _innerRadius;
		private float _outerRadius;
		
		private void Update()
		{
			if (_numSegments != NumSegments ||
			    _innerRadius != InnerRadius ||
			    _outerRadius != OuterRadius)
			{
				_numSegments = NumSegments;
				_innerRadius = InnerRadius;
				_outerRadius = OuterRadius;

				var filter = GetComponent<MeshFilter>();
				filter.mesh = CreateMesh();
			}
		}

		private Mesh CreateMesh()
		{
			var geometry = new Mesh();
			geometry.name = "Rings";

			var numVertices = NumSegments * 2;
			var numTriangles = NumSegments * 2;
			var numIndices = numTriangles * 3;
			var step = Mathf.PI * 2 / NumSegments;

			var vertices = new Vector3[numVertices];
			var uv = new Vector2[numVertices];
			var normals = new Vector3[numVertices];

			var tris = new int[numIndices];

			for (int i = 0; i < NumSegments; ++i)
			{
				var innerVertexIndex = i * 2;
				var outerVertexIndex = i * 2 + 1;
				var nextInnerVertexIndex = i * 2 + 2;
				var nextOuterVertexIndex = i * 2 + 3;
				if (i >= NumSegments - 1)
				{
					nextInnerVertexIndex = 0;
					nextOuterVertexIndex = 1;
				}

				var angle = i * step;
				var delta = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle));

				vertices[innerVertexIndex] = delta * InnerRadius;
				vertices[outerVertexIndex] = delta * OuterRadius;

				uv[innerVertexIndex] = new Vector2(0, 0);
				uv[outerVertexIndex] = new Vector2(1, 1);

				normals[innerVertexIndex] = Vector3.up;
				normals[outerVertexIndex] = Vector3.up;

				tris[i * 6] = innerVertexIndex;
				tris[i * 6 + 1] = nextOuterVertexIndex;
				tris[i * 6 + 2] = outerVertexIndex;

				tris[i * 6 + 3] = innerVertexIndex;
				tris[i * 6 + 4] = nextInnerVertexIndex;
				tris[i * 6 + 5] = nextOuterVertexIndex;
			}
			geometry.vertices = vertices;
			geometry.uv = uv;
			geometry.triangles = tris;

			return geometry;
		}
	}
}