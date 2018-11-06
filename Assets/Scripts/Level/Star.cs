using UnityEditor;
using UnityEngine;

//public enum AgiltyBoost { HigherJump, WallJump};
public enum AgiltyBoost { HigherJump };

[ExecuteInEditMode]
public class Star : MonoBehaviour
{
    public AgiltyBoost TypeBoost
    {
        get { return _TypeBoost; }

        set { _TypeBoost = value; }
    }
    [SerializeField]
    private AgiltyBoost _TypeBoost = AgiltyBoost.HigherJump;

    public float JumpBoost
    {
        get { return _JumpBoost; }

        set { _JumpBoost = value; }
    }
    [SerializeField]
    private float _JumpBoost = 2.0f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player") return;
        other.GetComponent<PlayerController>().UseStar(this);
        gameObject.SetActive(false);
    }
}

[CustomEditor(typeof(Star))]
[CanEditMultipleObjects]
public class StarManagerEditor : Editor
{

    SerializedProperty TypeBoost;
    SerializedProperty JumpBoost;


    private void OnEnable()
    {
        TypeBoost = serializedObject.FindProperty("_TypeBoost");
        JumpBoost = serializedObject.FindProperty("_JumpBoost");
    }
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(TypeBoost);
        switch (TypeBoost.intValue)
        {
            case (int)AgiltyBoost.HigherJump:
                EditorGUILayout.PropertyField(JumpBoost);
                break;

            //case (int)AgiltyBoost.WallJump:

            //    break;

            default:
                break;
        }
        serializedObject.ApplyModifiedProperties();

    }
}