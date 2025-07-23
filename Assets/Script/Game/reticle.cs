using UnityEngine;


public class ReticleController : MonoBehaviour
{
    public Transform reticle;                  // レティクルのTransform
    public Vector3 targetPosition;             // レティクルを表示する座標
    public float detectionRadius = 2.0f;       // レティクルの範囲
    public float requiredStayTime = 5.0f;      // 停止までに必要な滞在時間

    private float stayTimer = 0f;
    private Animator targetAnimator;
    private ParticleSystem targetParticles;

    void Update()
    {
        // レティクルを指定座標に移動
        if (reticle != null)
        {
            reticle.position = targetPosition;
        }

        // 範囲内にアニメーションオブジェクトがあるか判定
        Collider[] hits = Physics.OverlapSphere(targetPosition, detectionRadius);
        bool targetFound = false;

        foreach (Collider hit in hits)
        {
            Animator anim = hit.GetComponent<Animator>();
            ParticleSystem particles = hit.GetComponent<ParticleSystem>();
            var particleRenderer = targetParticles.GetComponent<Renderer>();

            if (anim != null || particles != null)
            {
                targetFound = true;

                if (targetAnimator != anim)
                {
                    targetAnimator = anim;
                    targetParticles = particles;
                    stayTimer = 0f;
                }
                else
                {
                    stayTimer += Time.deltaTime;
                    if (stayTimer >= requiredStayTime)
                    {
                        if (targetAnimator != null)
                        {
                            targetAnimator.enabled = false;
                            Debug.Log("アニメーション停止: " + hit.name);
                        }

                        
                        if (particleRenderer != null && particleRenderer.enabled)
                        {
                            particleRenderer.enabled = false;
                            Debug.Log("パーティクル描画を非表示: " + hit.name);
                        }
                    }
                }

                break;
            }
        }

        if (!targetFound)
        {
            stayTimer = 0f;
            targetAnimator = null;
        }
    }

    void OnDrawGizmosSelected()
    {
        // レティクル範囲の表示（エディタ上のみ）
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(targetPosition, detectionRadius);
    }
}