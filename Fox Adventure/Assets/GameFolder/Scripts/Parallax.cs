using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    private MeshRenderer mesh;
    // Start is called before the first frame update
    [Header("Parallax em X")]
    [Range(0.1f,10f)]
    [Tooltip("Quanto menor o valor maior o parallax em X")]
    public float xParallax = 10f;

    [Header("Parallax em Y")]
    [Range(0f,5f)]
    [Tooltip("Quanto maior o valor maior o parallax em Y")]
    public float yParallax = 0f;
    private float yPos;         // somatoria das variaveis do parallax
    private Vector2 startPos;   // posicao do background na camera
    private float horizontal;   // cria o efeito em parallax
    void Start()
    {
        mesh = GetComponent<MeshRenderer>();

        startPos = new Vector2(transform.position.x, transform.position.y);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        // parallax em X do mainTextureOffset
        mesh.material.mainTextureOffset = new Vector2(Camera.main.transform.position.x/(xParallax*10f),0f);

        // parallax em Y do position do mainTextureOffset
        horizontal = (Camera.main.transform.position.y * (yParallax/10f));

        yPos = startPos.y + (Camera.main.transform.position.y - horizontal);

        transform.position = new Vector3(transform.position.x, yPos, transform.position.z);

    }
}
