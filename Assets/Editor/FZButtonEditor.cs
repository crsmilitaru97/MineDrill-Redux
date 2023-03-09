using UnityEditor;
using UnityEditor.UI;
using UnityEngine.UI;

//12.10.22

[CustomEditor(typeof(FZButton))]
public class FZButtonEditor : ButtonEditor
{
    public override void OnInspectorGUI()
    {
        FZButton targetButton = (FZButton)target;
        targetButton.playClickSound = EditorGUILayout.Toggle("Play Click Sound", targetButton.playClickSound);
        targetButton.buttonText = (Text)EditorGUILayout.ObjectField("Button Text", targetButton.buttonText, typeof(Text), true);
        targetButton.buttonImage = (Image)EditorGUILayout.ObjectField("Button Image", targetButton.buttonImage, typeof(Image), true);
        targetButton.selectedColor = EditorGUILayout.ColorField("Selected Color", targetButton.selectedColor);
        targetButton.isSelected = EditorGUILayout.Toggle("Selected", targetButton.isSelected);

        base.OnInspectorGUI();
    }
}
