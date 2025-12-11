using Sirenix.OdinInspector;
using UnityEngine;

namespace Henmova.Vaibhav
{
    public class Player_Referrences : MonoBehaviour
    {
        [Title("----- Custom Components -----", titleAlignment: TitleAlignments.Centered)]
        public Player_Detections PlayerDetections;
        public Player_Movement_FP PlayerMovementFP;
        [Title("----- Inbuilt Components -----", titleAlignment: TitleAlignments.Centered)]
        public Animator Animator;
    }
}
