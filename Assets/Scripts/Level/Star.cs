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

    [SerializeField]
    AudioClip audioClip;
    AudioSource source;

    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        source.PlayOneShot(audioClip, 0.5f);
        other.GetComponent<PlayerController>().UseStar(this);
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Collider>().enabled = false;
    }
}
/*
[CustomEditor(typeof(Star))]
[CanEditMultipleObjects]
public class StarManagerEditor : Editor
{
    MonoScript script;
    SerializedProperty TypeBoost;
    SerializedProperty JumpBoost;
    SerializedProperty AudioClip;


    private void OnEnable()
    {
        script = MonoScript.FromMonoBehaviour((Star)target);
        TypeBoost = serializedObject.FindProperty("_TypeBoost");
        JumpBoost = serializedObject.FindProperty("_JumpBoost");
        AudioClip = serializedObject.FindProperty("audioClip");
    }
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        script = EditorGUILayout.ObjectField(script, typeof(MonoScript), false) as MonoScript;
        EditorGUILayout.PropertyField(AudioClip);
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
}*/