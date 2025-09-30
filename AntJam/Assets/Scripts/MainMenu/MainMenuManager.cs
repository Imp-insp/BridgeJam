using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class MainMenuManager : MonoBehaviour
{
    [Header("Scene to Load")]
    [SerializeField] private string gameSceneName = "AntHill";

    [Header("Transition Settings")]
    [SerializeField] private AnimationCurve transitionCurve; // Curva para suavizar o movimento principal
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Image fadePanel;
    [SerializeField] private float transitionDuration = 2.5f;
    [SerializeField] private float shakeDuration = 0.5f;
    [SerializeField] private float shakeMagnitude = 0.1f;

    [Header("Effects")]
    // Use uma lista (array) para poder adicionar quantos sistemas de part�culas quiser.
    [SerializeField] private ParticleSystem[] dustParticles;

    private bool isTransitioning = false;
    private Vector3 cameraStartPosition;

    void Start()
    {
        if (mainCamera != null)
        {
            cameraStartPosition = mainCamera.transform.position;
        }
    }

    public void OnPlayButtonPressed()
    {
        if (isTransitioning) return;
        StartCoroutine(PlayTransitionCoroutine());
    }

    public void OnQuitButtonPressed()
    {
        Application.Quit();
    }

    private IEnumerator PlayTransitionCoroutine() // Anima��o do tremor, part�culas, fade in da tela preta e da c�mera descendo acontecem aqui!
    {
        isTransitioning = true;

        // Inicia todos os sistemas de part�culas que estiverem na lista.
        if (dustParticles != null)
        {
            foreach (ParticleSystem particle in dustParticles)
            {
                if (particle)
                {
                    particle.Play();
                }
            }
        }

        float elapsedTime = 0f;
        Vector3 targetCameraPos = new Vector3(cameraStartPosition.x, cameraStartPosition.y - 10.8f, cameraStartPosition.z);

        while (elapsedTime < transitionDuration)
        {
            elapsedTime += Time.deltaTime;
            float linearProgress = Mathf.Clamp01(elapsedTime / transitionDuration);
            float curvedProgress = transitionCurve.Evaluate(linearProgress);

            fadePanel.color = Color.Lerp(Color.clear, Color.black, curvedProgress);

            Vector3 currentPos = Vector3.Lerp(cameraStartPosition, targetCameraPos, curvedProgress);
            mainCamera.transform.position = currentPos;

            if (elapsedTime < shakeDuration)
            {
                // A magnitude do tremor agora aumenta seguindo a curva
                float currentShakeMagnitude = shakeMagnitude * linearProgress;
                float x = Random.Range(-1f, 1f) * currentShakeMagnitude;
                float y = Random.Range(-1f, 1f) * currentShakeMagnitude;
                mainCamera.transform.position += new Vector3(x, y, 0);
            }

            yield return null;
        }

        mainCamera.transform.position = targetCameraPos;
        fadePanel.color = Color.black;
        SceneManager.LoadScene(gameSceneName);
    }
}