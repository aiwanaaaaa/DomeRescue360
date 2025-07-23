using UnityEngine;


public class ReticleController : MonoBehaviour
{
    public Transform reticle;                  // ���e�B�N����Transform
    public Vector3 targetPosition;             // ���e�B�N����\��������W
    public float detectionRadius = 2.0f;       // ���e�B�N���͈̔�
    public float requiredStayTime = 5.0f;      // ��~�܂łɕK�v�ȑ؍ݎ���

    private float stayTimer = 0f;
    private Animator targetAnimator;
    private ParticleSystem targetParticles;

    void Update()
    {
        // ���e�B�N�����w����W�Ɉړ�
        if (reticle != null)
        {
            reticle.position = targetPosition;
        }

        // �͈͓��ɃA�j���[�V�����I�u�W�F�N�g�����邩����
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
                            Debug.Log("�A�j���[�V������~: " + hit.name);
                        }

                        
                        if (particleRenderer != null && particleRenderer.enabled)
                        {
                            particleRenderer.enabled = false;
                            Debug.Log("�p�[�e�B�N���`����\��: " + hit.name);
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
        // ���e�B�N���͈͂̕\���i�G�f�B�^��̂݁j
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(targetPosition, detectionRadius);
    }
}