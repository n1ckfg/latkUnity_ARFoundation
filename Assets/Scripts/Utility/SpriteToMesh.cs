using UnityEngine;
using System.Collections;

//[ExecuteInEditMode]
public class SpriteToMesh : MonoBehaviour {

	public bool hideOriginal = true;
	public bool spriteIsPacked = false;
	public SpriteRenderer sourceSprite;
	public Renderer[] destMesh;

	private bool firstRun = true;

	void Start() {
		sourceSprite = GetComponent<SpriteRenderer>();
		if (hideOriginal) sourceSprite.enabled = false;
	}
	
	void Update() {
		if (firstRun) {
			firstRun = false;
		} else {
			for (int i = 0; i < destMesh.Length; i++) {
				if (spriteIsPacked) {
					destMesh[i].sharedMaterial.mainTexture = grabPackedSprite(sourceSprite.sprite);
				} else {
					destMesh[i].sharedMaterial.mainTexture = sourceSprite.sprite.texture;
				}
			}
		}
	}

	Texture2D grabPackedSprite(Sprite _sprite) {
		// Make sure read/write is enabled in Advanced import settings
		Texture2D tex = new Texture2D((int) _sprite.rect.width, (int) _sprite.rect.height);
		Color[] pixels = _sprite.texture.GetPixels((int) _sprite.textureRect.x, (int) _sprite.textureRect.y, (int) _sprite.textureRect.width, (int) _sprite.textureRect.height);
		tex.SetPixels(pixels);
		tex.Apply();
		return tex;
	}

}
