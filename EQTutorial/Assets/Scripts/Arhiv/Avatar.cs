/* Avatar - Each character should have things like eye position and other elements, all easily grabbed from here
 * Created - March 3 2013
 * PegLegPete (goatdude@gmail.com)
 */

using UnityEngine;
using System.Collections;

namespace EQBrowser
{
    public class Avatar : MonoBehaviour
    {
        CharacterController m_controller;
        protected Vector3 m_eyePosition;
        public Vector3 EyePosition
        {
            get { return m_eyePosition; }
            set { m_eyePosition = value; }
        }

        protected Rigidbody m_rigidBody;
        public Rigidbody Rigidbody
        {
            get { return m_rigidBody; }
        }

        //Animations
        public enum AnimationType
        {
            Kick,
            Melee1HP,
            Melee2HS,
            Melee2HB,
            Melee1H,
            MeleeOffhand,
            MeleeBash,
            MeleeH2H,
            KickFlying,
            MeleeEagleStrike,
            MeleeDragonPunch,
            KickRound,
            Ranged,
            SwimAttack,
            Hurt,
            Hurt_Big,
            Drowning,
            Dead,
            Walk,
            Run,
            RunJump,
            Jump,
            Fall,
            Sneak,
            Climb,
            Crouch,
            TreadWater,
            Idle,
            Idle_Alt,
            Sit,
            SitIdle,
            Turn,
            Loot,
            Swim,
            SocialCheer,
            SocialCry,
            SocialWave,
            SocialRude,
            InstrumentStringed,
            InstrumentWind,
            CastBuff,
            CastHeal,
            CastDamage,
            DeadIdle,
            CrouchIdle,
            //..
            DEFAULT
        }

        #region anim
        public AnimationClip defaultAnimation;
        public AnimationClip[] characterAnimations;

        //public AnimationClip DEFAULT; //Play this animation (like a cast) if this model doesn't have the desired one
        //public AnimationClip anim_MeleeKick;
        //public AnimationClip anim_Melee1HP;
        //public AnimationClip anim_Melee2HS;
        //public AnimationClip anim_Melee2HB;
        //public AnimationClip anim_Melee1H;
        //public AnimationClip anim_MeleeOffhand;
        //public AnimationClip anim_MeleeBash;
        //public AnimationClip anim_MeleeH2H;
        //public AnimationClip anim_MeleeKickFlying;
        //public AnimationClip anim_MeleeEagleStrike;
        //public AnimationClip anim_MeleeDragonPunch;
        //public AnimationClip anim_Ranged;
        //public AnimationClip anim_SwimAttack;
        //public AnimationClip anim_KickRound;
        //public AnimationClip anim_Hurt;
        //public AnimationClip anim_Hurt_Big;
        //public AnimationClip anim_Drowning;
        //public AnimationClip anim_Dead;
        //public AnimationClip anim_Walk;
        //public AnimationClip anim_Run;
        //public AnimationClip anim_RunJump;
        //public AnimationClip anim_Jump;
        //public AnimationClip anim_Fall;
        //public AnimationClip anim_Sneak;
        //public AnimationClip anim_Climb;
        //public AnimationClip anim_Crouch;
        //public AnimationClip anim_TreadWater;
        //public AnimationClip anim_Idle;
        //public AnimationClip anim_Idle_Alt;
        //public AnimationClip anim_Sit;
        //public AnimationClip anim_Turn;
        //public AnimationClip anim_Loot;
        //public AnimationClip anim_Swim;
        //public AnimationClip anim_SocialCheer;
        //public AnimationClip anim_SocialCry;
        //public AnimationClip anim_SocialWave;
        //public AnimationClip anim_SocialRude;
        //public AnimationClip anim_InstrumentStringed;
        //public AnimationClip anim_InstrumentWind;
        //public AnimationClip anim_CastBuff;
        //public AnimationClip anim_CastHeal;
        //public AnimationClip anim_CastDamage;
#endregion

        Vector3 m_prevPos;
        Vector3 m_curPos;
        float m_lastVertHeight;
        float m_animSpeed = 1.0f;

        public bool m_isDead;
        public bool m_isSitting;
        public bool m_isCrouching;

        public float m_movementSpeed = 15f;
        public float turnRate = 25f;

        public enum AnimStates
        {
            Idle, //Idle depending on what's going on (idle or swimming or falling)
            Turning,
            Falling,
            MovingForward, //Moving forward
            MovingBackward, //Moving backward
            MovingSideWays, //Strafing
            Melee, //Performing one of the melee states
            Casting, //Casting one of the possible spells
            Social, //Performing an emote
            Sitting,
            Ducking,
            Jump
        }

        public enum AnimTransitions
        {
            None,
            SittingDown,
            StandingUp,
            DuckingDown,
            DuckingUp,
            Jumping,
            Falling,
            Attack,
            Dying,
            UnDying, //Getting back up, like from a FD
        }

        public enum TurningType
        {
            None,
            ClockWise,
            CClockWise
        }

        public enum AnimAttackType
        {
            None,
            M1H,
            M1HP,
            MOH,
            M2HB,
            M2HS,
            MKick,
            MKickRound,
            MKickFlying,
            MRanged,
            MEagleStrike,
            MDragonPunch,
            CastBuff,
            CastHeal,
            CastDamage
        }

        AnimStates m_curState;
        AnimTransitions m_curAnimTransition;
        Animation m_animComponent;
        AnimAttackType m_attackType;
        TurningType m_turnDir;
        string m_curAnimName;

        float m_animTimer;

        void Start()
        {
            m_controller = this.GetComponentInChildren<CharacterController>();
            m_curState = AnimStates.Idle;
            m_animComponent = this.gameObject.GetComponentInChildren<Animation>();

            m_isDead = false;
            m_isSitting = false;

            for (int i = 0; i < (int)AnimationType.DEFAULT; i++)
            {
                m_animComponent.AddClip(characterAnimations[i], ((AnimationType)i).ToString());
            }

            m_animComponent.Play(characterAnimations[(int)AnimationType.Idle].name);

            m_animComponent.wrapMode = WrapMode.Loop;
            
            m_attackType = AnimAttackType.None;
            m_turnDir = TurningType.None;
        }

        void Update()
        {
            ProcessTestAnimationKeyPresses();

            m_animSpeed = 1.0f;

            AnimState();

            m_curState = AnimStates.Idle;
            m_turnDir = TurningType.None;

            m_curPos = this.gameObject.transform.position;

            if (!Mathf.Approximately(m_prevPos.x, m_curPos.x) || !Mathf.Approximately(m_prevPos.z, m_curPos.z))
            {
                //Not the most efficient route, but plot a point based on current facing
                Vector3 nextPoint = m_curPos + (this.gameObject.transform.forward);
                //Then get the distance between prev point and "next" point
                float distPrev = Vector3.Distance(m_prevPos, m_curPos);
                float distNext = Vector3.Distance(nextPoint, m_curPos);
                //if (Vector3.Dot(m_curPos - m_prevPos, Vector3.forward) > 0.0f)
                //If we're moving forward then distance between cur and next should always be less than cur and prev
                if(distNext < distPrev)
                {
                    m_curState = AnimStates.MovingBackward;
                }
                else
                {
                    m_curState = AnimStates.MovingForward;
                }
            }
            //{
            //    m_curState = AnimStates.Moving;
            //}

            //If the last grounded height is > 75% of character height, we're falling
            if (m_lastVertHeight - m_curPos.y > (m_controller.height * 0.75f))
            {
                m_curState = AnimStates.Falling;
            }
            //if (!Mathf.Approximately(m_prevPos.y, m_curPos.y))
            //{
            //    m_curState = AnimStates.Falling;
            //}

            m_prevPos = m_curPos;
        }

        void AnimState()
        {
            string animName = characterAnimations[(int)AnimationType.Idle].name;

            switch(m_curAnimTransition)
            {
                case AnimTransitions.Jumping:
                    //Run jump if moving
                    if (m_curState == AnimStates.MovingForward || m_curState == AnimStates.MovingBackward || m_curState == AnimStates.MovingSideWays)
                    {
                        animName = characterAnimations[(int)AnimationType.RunJump].name;
                        m_animTimer = characterAnimations[(int)AnimationType.RunJump].length;
                    }
                    //else just jump
                    else
                    {
                        animName = characterAnimations[(int)AnimationType.Jump].name;
                        m_animTimer = characterAnimations[(int)AnimationType.Jump].length;
                    }
                    break;
                case AnimTransitions.Attack:
                    #region Attack Breakdown
                    switch (m_attackType)
                    {
                        case AnimAttackType.CastBuff:
                            animName = characterAnimations[(int)AnimationType.CastBuff].name;
                            m_animTimer = characterAnimations[(int)AnimationType.CastBuff].length;
                            break;
                        case AnimAttackType.CastDamage:
                            animName = characterAnimations[(int)AnimationType.CastDamage].name;
                            m_animTimer = characterAnimations[(int)AnimationType.CastDamage].length;
                            break;
                        case AnimAttackType.CastHeal:
                            animName = characterAnimations[(int)AnimationType.CastHeal].name;
                            m_animTimer = characterAnimations[(int)AnimationType.CastHeal].length;
                            break;
                        case AnimAttackType.M1H:
                            animName = characterAnimations[(int)AnimationType.Melee1H].name;
                            m_animTimer = characterAnimations[(int)AnimationType.Melee1H].length;
                            break;
                        case AnimAttackType.M1HP:
                            animName = characterAnimations[(int)AnimationType.Melee1HP].name;
                            m_animTimer = characterAnimations[(int)AnimationType.Melee1HP].length;
                            break;
                        case AnimAttackType.M2HB:
                            animName = characterAnimations[(int)AnimationType.Melee2HB].name;
                            m_animTimer = characterAnimations[(int)AnimationType.Melee2HB].length;
                            break;
                        case AnimAttackType.M2HS:
                            animName = characterAnimations[(int)AnimationType.Melee2HS].name;
                            m_animTimer = characterAnimations[(int)AnimationType.Melee2HS].length;
                            break;
                        case AnimAttackType.MDragonPunch:
                            animName = characterAnimations[(int)AnimationType.MeleeDragonPunch].name;
                            m_animTimer = characterAnimations[(int)AnimationType.MeleeDragonPunch].length;
                            break;
                        case AnimAttackType.MEagleStrike:
                            animName = characterAnimations[(int)AnimationType.MeleeEagleStrike].name;
                            m_animTimer = characterAnimations[(int)AnimationType.MeleeEagleStrike].length;
                            break;
                        case AnimAttackType.MKick:
                            animName = characterAnimations[(int)AnimationType.Kick].name;
                            m_animTimer = characterAnimations[(int)AnimationType.Kick].length;
                            break;
                        case AnimAttackType.MKickFlying:
                            animName = characterAnimations[(int)AnimationType.KickFlying].name;
                            m_animTimer = characterAnimations[(int)AnimationType.KickFlying].length;
                            break;
                        case AnimAttackType.MKickRound:
                            animName = characterAnimations[(int)AnimationType.KickRound].name;
                            m_animTimer = characterAnimations[(int)AnimationType.KickRound].length;
                            break;
                        case AnimAttackType.MOH:
                            animName = characterAnimations[(int)AnimationType.MeleeOffhand].name;
                            m_animTimer = characterAnimations[(int)AnimationType.MeleeOffhand].length;
                            break;
                        case AnimAttackType.MRanged:
                            animName = characterAnimations[(int)AnimationType.Ranged].name;
                            m_animTimer = characterAnimations[(int)AnimationType.Ranged].length;
                            break;
                    }
                    #endregion
                    //animName = characterAnimations[(int)AnimationType.MeleeDragonPunch].name;
                    //m_animTimer = characterAnimations[(int)AnimationType.MeleeDragonPunch].length;
                    break;
                case AnimTransitions.Dying:
                    animName = characterAnimations[(int)AnimationType.Dead].name;
                    m_animTimer = characterAnimations[(int)AnimationType.Dead].length;
                    m_animComponent.GetComponent<Animation>()["Dead"].speed = 1.0f;
                    break;
                case AnimTransitions.UnDying:
                    animName = characterAnimations[(int)AnimationType.Dead].name;
                    m_animTimer = characterAnimations[(int)AnimationType.Dead].length;
                    m_animComponent.GetComponent<Animation>()["Dead"].speed = -1.0f;
                    break;
                case AnimTransitions.SittingDown:
                    animName = characterAnimations[(int)AnimationType.Sit].name;
                    m_animTimer = characterAnimations[(int)AnimationType.Sit].length;
                    m_animComponent.GetComponent<Animation>()["Sit"].speed = 1.0f;
                    break;
                case AnimTransitions.StandingUp:
                    animName = characterAnimations[(int)AnimationType.Sit].name;
                    m_animTimer = characterAnimations[(int)AnimationType.Sit].length;
                    m_animComponent.GetComponent<Animation>()["Sit"].speed = -1.0f;
                    break;
            }

            if(m_curAnimTransition != AnimTransitions.None)
            {
                m_animComponent.Play(animName);
                m_curAnimTransition = AnimTransitions.None;
                m_curAnimName = animName;
            }

            if (m_animTimer > 0)
            {
                m_animTimer -= Time.deltaTime;
                return;
            }

            switch (m_curState)
            {
                case AnimStates.Idle:
                    if (m_isDead) 
                    {
                        animName = characterAnimations[(int)AnimationType.DeadIdle].name;
                    }
                    else if (m_isSitting)
                    {
                        animName = characterAnimations[(int)AnimationType.SitIdle].name;
                    }
                    else if (m_isCrouching)
                    {
                        animName = characterAnimations[(int)AnimationType.CrouchIdle].name;
                    }
                    else
                    {
                        animName = characterAnimations[(int)AnimationType.Idle].name;
                    }
                    break;
                case AnimStates.Turning:
                    if (m_turnDir == TurningType.ClockWise)
                    {
                        animName = characterAnimations[(int)AnimationType.Turn].name;
                    }
                    else if (m_turnDir == TurningType.CClockWise)
                    {
                        animName = characterAnimations[(int)AnimationType.Turn].name;
                    }
                    break;
                case AnimStates.Falling:
                    animName = characterAnimations[(int)AnimationType.Fall].name;
                    break;
                case AnimStates.MovingForward:
                    if (m_isCrouching)
                    {
                        animName = characterAnimations[(int)AnimationType.Sneak].name;
                        m_animComponent.GetComponent<Animation>()["Sneak"].speed = 1.0f;
                    }
                    else
                    {
                        animName = characterAnimations[(int)AnimationType.Run].name;
                        m_animComponent.GetComponent<Animation>()["Run"].speed = 1.0f;
                    }
                    break;
                case AnimStates.MovingBackward:
                    if (m_isCrouching)
                    {
                        animName = characterAnimations[(int)AnimationType.Sneak].name;
                        m_animComponent.GetComponent<Animation>()["Sneak"].speed = -1.0f;
                    }
                    else
                    {
                        animName = characterAnimations[(int)AnimationType.Run].name;
                        m_animComponent.GetComponent<Animation>()["Run"].speed = -1.0f;
                    }
                    break;
            }

            if (m_curAnimName != animName)
            {
                m_animComponent.Play(animName);
                m_curAnimName = animName;
            }
        }

        void ProcessTestAnimationKeyPresses()
        {
            m_attackType = AnimAttackType.None;

            bool shift = Input.GetKey(KeyCode.LeftShift);
            bool ctrl = Input.GetKey(KeyCode.LeftControl);

            if (shift)
            {
                if (Input.GetKeyDown(KeyCode.Alpha1))
                {
                    m_attackType = AnimAttackType.CastBuff;
                    m_curAnimTransition = AnimTransitions.Attack;
                }
                else if (Input.GetKeyDown(KeyCode.Alpha2))
                {
                    m_attackType = AnimAttackType.CastDamage;
                    m_curAnimTransition = AnimTransitions.Attack;
                }
                else if (Input.GetKeyDown(KeyCode.Alpha3))
                {
                    m_attackType = AnimAttackType.CastHeal;
                    m_curAnimTransition = AnimTransitions.Attack;
                }

                if (Input.GetKeyDown(KeyCode.C))
                {
                    if (m_curAnimTransition == AnimTransitions.None)
                    {
                        if (!m_isSitting)
                        {
                            m_curAnimTransition = AnimTransitions.SittingDown;
                        }
                        else
                        {
                            m_curAnimTransition = AnimTransitions.StandingUp;
                        }
                        m_isSitting = !m_isSitting;
                        
                    }
                }
                else if (Input.GetKeyDown(KeyCode.X))
                {
                    if (m_curAnimTransition == AnimTransitions.None)
                    {
                        if (!m_isDead)
                        {
                            m_curAnimTransition = AnimTransitions.Dying;
                        }
                        else
                        {
                            m_curAnimTransition = AnimTransitions.UnDying;
                        }
                        m_isDead = !m_isDead;
                    }
                }
                else if (Input.GetKeyDown(KeyCode.V))
                {
                    if (m_curAnimTransition == AnimTransitions.None)
                    {
                        if (!m_isCrouching)
                        {
                            m_curAnimTransition = AnimTransitions.DuckingDown;
                        }
                        else
                        {
                            m_curAnimTransition = AnimTransitions.DuckingUp;
                        }
                        m_isCrouching = !m_isCrouching;
                    }
                }
            }
                //NOTE: Since Unity uses CTRL + KEY in the editor, these will not work
            else if (ctrl)
            {
                if (Input.GetKeyDown(KeyCode.C))
                {
                    if (m_curAnimTransition == AnimTransitions.None)
                    {
                        if (!m_isSitting)
                        {
                            m_curAnimTransition = AnimTransitions.SittingDown;
                        }
                        else
                        {
                            m_curAnimTransition = AnimTransitions.StandingUp;
                        }
                        m_isSitting = !m_isSitting;

                    }
                }
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.Alpha1))
                {
                    m_attackType = AnimAttackType.M1H;
                    m_curAnimTransition = AnimTransitions.Attack;
                }
                else if (Input.GetKeyDown(KeyCode.Alpha2))
                {
                    m_attackType = AnimAttackType.MOH;
                    m_curAnimTransition = AnimTransitions.Attack;
                }
                else if (Input.GetKeyDown(KeyCode.Alpha3))
                {
                    m_attackType = AnimAttackType.M1HP;
                    m_curAnimTransition = AnimTransitions.Attack;
                }
                else if (Input.GetKeyDown(KeyCode.Alpha4))
                {
                    m_attackType = AnimAttackType.M2HB;
                    m_curAnimTransition = AnimTransitions.Attack;
                }
                else if (Input.GetKeyDown(KeyCode.Alpha5))
                {
                    m_attackType = AnimAttackType.M2HS;
                    m_curAnimTransition = AnimTransitions.Attack;
                }
                else if (Input.GetKeyDown(KeyCode.Alpha6))
                {
                    m_attackType = AnimAttackType.MDragonPunch;
                    m_curAnimTransition = AnimTransitions.Attack;
                }
                else if (Input.GetKeyDown(KeyCode.Alpha7))
                {
                    m_attackType = AnimAttackType.MEagleStrike;
                    m_curAnimTransition = AnimTransitions.Attack;
                }
                else if (Input.GetKeyDown(KeyCode.Alpha8))
                {
                    m_attackType = AnimAttackType.MKick;
                    m_curAnimTransition = AnimTransitions.Attack;
                }
                else if (Input.GetKeyDown(KeyCode.Alpha9))
                {
                    m_attackType = AnimAttackType.MKickFlying;
                    m_curAnimTransition = AnimTransitions.Attack;
                }
                else if (Input.GetKeyDown(KeyCode.Alpha0))
                {
                    m_attackType = AnimAttackType.MKickRound;
                    m_curAnimTransition = AnimTransitions.Attack;
                }
                else if (Input.GetKeyDown(KeyCode.Minus))
                {
                    m_attackType = AnimAttackType.MRanged;
                    m_curAnimTransition = AnimTransitions.Attack;
                }
            }
        }

        public void Jump()
        {
            if (m_curAnimTransition == AnimTransitions.None)
            {
                m_curAnimTransition = AnimTransitions.Jumping;
            }
        }

        public void CharTurning(bool clockwise)
        {
            if (m_isCrouching)
                return;

           // zer0sum: dont play turn anim if we're running...
           // if (Input.GetKey(KeyCode.W) || (Input.GetMouseButton(0) && Input.GetMouseButtonDown(1)))
            if (m_curState == AnimStates.MovingForward || m_curState == AnimStates.MovingSideWays || m_curState == AnimStates.MovingBackward)
                return;

            m_curState = AnimStates.Turning;
            if (clockwise)
            {
                m_turnDir = TurningType.ClockWise;
            }
            else
            {
                m_turnDir = TurningType.CClockWise;
            }
        }

        /// <summary>
        /// Last/current ground height, we'll compare against this value to see if we're falling
        /// </summary>
        /// <param name="height"></param>
        public void SetCurGroundHeight(float height)
        {
            m_lastVertHeight = height;
        }

        //string animName = characterAnimations[(int)AnimationType.Idle].name;

        //    bool allowCrossfade = true;

        //    switch (m_goalAnimState)
        //    {
        //        case CurAnimState.Moving:
        //            animName = characterAnimations[(int)AnimationType.Run].name; 
        //            break;
        //        case CurAnimState.Idle:
        //            animName = characterAnimations[(int)AnimationType.Idle].name;
        //            break;
        //        case CurAnimState.Jump:
        //            animName = characterAnimations[(int)AnimationType.Jump].name;
        //            allowCrossfade = false;
        //            break;
        //    }

        //    if (m_curAnimState != m_goalAnimState)
        //    {
        //        if (allowCrossfade)
        //        {
        //            m_animComponent.CrossFade(animName);
        //        }
        //        else
        //        {
        //            m_animComponent.Play(animName);
        //        }
        //        m_curAnimState = m_goalAnimState;
        //        m_goal

        //    }
    }   
}