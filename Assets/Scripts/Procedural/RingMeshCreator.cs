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
				filter.mesh = CreateMesh(_numSegments, _innerRadius, _outerRadius);
			}
		}

		private static Mesh CreateMesh(int numSegments, float innerRadius, float outerRadius)
		{
			var geometry = new Mesh();
			geometry.name = "Rings";

			var numVerticesPerSide = numSegments * 2;
			var numVertices = 2 * numVerticesPerSide;
			var numTriangles = numSegments * 2;
			var numIndicesPerSide = numTriangles * 3;
			var numIndices = 2 * numIndicesPerSide;
			var step = Mathf.PI * 2 / numSegments;

			var vertices = new Vector3[numVertices];
			var uv = new Vector2[numVertices];
			var normals = new Vector3[numVertices];

			var tris = new int[numIndices];

			for (int i = 0; i < numSegments; ++i)
			{
				var innerVertexIndex = i * 2;
				var outerVertexIndex = i * 2 + 1;
				var nextInnerVertexIndex = i * 2 + 2;
				var nextOuterVertexIndex = i * 2 + 3;
				if (i >= numSegments - 1)
				{
					nextInnerVertexIndex = 0;
					nextOuterVertexIndex = 1;
				}

				var angle = i * step;
				var delta = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle));

				vertices[innerVertexIndex] = delta * innerRadius;
				vertices[outerVertexIndex] = delta * outerRadius;

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

			int vertexStart = 2 * numSegments;
			int indexStart = 6 * numSegments;
			for (int i = 0; i < numSegments; ++i)
			{
				var innerVertexIndex = vertexStart + i * 2;
				var outerVertexIndex = vertexStart + i * 2 + 1;
				var nextInnerVertexIndex = vertexStart + i * 2 + 2;
				var nextOuterVertexIndex = vertexStart + i * 2 + 3;
				if (i >= numSegments - 1)
				{
					nextInnerVertexIndex = vertexStart;
					nextOuterVertexIndex = vertexStart + 1;
				}

				var angle = i * step;
				var delta = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle));

				vertices[innerVertexIndex] = delta * innerRadius;
				vertices[outerVertexIndex] = delta * outerRadius;

				uv[innerVertexIndex] = new Vector2(0, 0);
				uv[outerVertexIndex] = new Vector2(1, 1);

				normals[innerVertexIndex] = Vector3.down;
				normals[outerVertexIndex] = Vector3.down;

				tris[indexStart + i * 6] = innerVertexIndex;
				tris[indexStart + i * 6 + 1] = outerVertexIndex;
				tris[indexStart + i * 6 + 2] = nextOuterVertexIndex;

				tris[indexStart + i * 6 + 3] = nextOuterVertexIndex;
				tris[indexStart + i * 6 + 4] = nextInnerVertexIndex;
				tris[indexStart + i * 6 + 5] = innerVertexIndex;
			}

			geometry.vertices = vertices;
			geometry.uv = uv;
			geometry.triangles = tris;

			return geometry;
		}
	}
}