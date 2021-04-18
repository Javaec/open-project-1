using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WaypointData
{
	public Vector3 waypoint;
	public List<Vector3> corners;
}

[CreateAssetMenu(fileName = "PathwayConfig", menuName = "EntityConfig/Pathway Config")]
public class PathwayConfigSO : NPCMovementConfigSO
{
	[HideInInspector] public List<WaypointData> Waypoints;

#if UNITY_EDITOR

	[SerializeField] Color _lineColor = Color.black;

	[SerializeField] [Range(0, 100)] int _textSize = 20;

	[SerializeField] Color _textColor = Color.white;

	[SerializeField]
	[Range(0, 100)]
	[Tooltip("This function may reduce the frame rate if a large probe radius is specified. To avoid frame rate issues," +
	         " it is recommended that you specify a max distance of twice the agent height.")]
	float _probeRadius = 3;

	[HideInInspector] public bool DisplayProbes;

	[HideInInspector] public bool ToggledNavMeshDisplay;

	List<Vector3> _path;
	List<bool> _hits;

	public const string FIELD_LABEL = "Point ";
	public const string TITLE_LABEL = "Waypoints";

	public Color LineColor
	{
		get
		{
			return _lineColor;
		}
	}

	public Color TextColor
	{
		get
		{
			return _textColor;
		}
	}

	public int TextSize
	{
		get
		{
			return _textSize;
		}
	}

	public float ProbeRadius
	{
		get
		{
			return _probeRadius;
		}
	}

	public List<Vector3> Path
	{
		get
		{
			return _path;
		}
		set
		{
			_path = value;
		}
	}

	public List<bool> Hits
	{
		get
		{
			return _hits;
		}
		set
		{
			_hits = value;
		}
	}

	public bool RealTimeEnabled;

#endif
}
