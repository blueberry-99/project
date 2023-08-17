using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    AudioSource AudioSource;

    [SerializeField] List<AudioClip> WalkSound_Grass = new List<AudioClip>();
    [SerializeField][Range(0, 1)] float WalkSound_Grass_Volume;
    [SerializeField] List<AudioClip> DashSound = new List<AudioClip>();
    [SerializeField][Range(0, 1)] float DashSound_Volume;
    [SerializeField] List<AudioClip> RunSound_Grass = new List<AudioClip>();
    [SerializeField][Range(0, 1)] float RunSound_Grass_Volume;
    [SerializeField] List<AudioClip> RunStopSound_Grass = new List<AudioClip>();
    [SerializeField][Range(0, 1)] float RunStopSound_Grass_Volume;

    [SerializeField] List<AudioClip> RollSound = new List<AudioClip>();
    [SerializeField][Range(0, 1)] float RollSound_Volume;

    [SerializeField] List<AudioClip> InAirDashSound = new List<AudioClip>();
    [SerializeField][Range(0, 1)] float InAirDashSound_Volume;

    [SerializeField] List<AudioClip> JumpSound_Grass = new List<AudioClip>();
    [SerializeField][Range(0, 1)] float JumpSound_Grass_Volume;
    [SerializeField] List<AudioClip> WallJumpSound_Grass = new List<AudioClip>();
    [SerializeField][Range(0, 1)] float WallJumpSound_Grass_Volume;

    [SerializeField] List<AudioClip> WallSlideSound_Grass = new List<AudioClip>();
    [SerializeField][Range(0, 1)] float WallSlideSound_Grass_Volume;

    [SerializeField] List<AudioClip> LandSound_Grass = new List<AudioClip>();
    [SerializeField][Range(0, 1)] float LandSound_Grass_Volume;
    [SerializeField] List<AudioClip> BaskAttack_Swing = new List<AudioClip>();
    [SerializeField][Range(0, 1)] float BaskAttack_Swing_Volume;
    [SerializeField] List<AudioClip> BaskAttack_EnemyHit_0 = new List<AudioClip>();
    [SerializeField][Range(0, 1)] float BaskAttack_EnemyHit_0_Volume;

    [SerializeField] List<AudioClip> Skill_Range_0_Pre = new List<AudioClip>();
    [SerializeField][Range(0, 1)] float Skill_Range_0_Pre_Volume;
    [SerializeField] List<AudioClip> Skill_Range_0_Fire = new List<AudioClip>();
    [SerializeField][Range(0, 1)] float Skill_Range_0_Fire_Volume;
    [SerializeField] List<AudioClip> Skill_Range_0_Hit = new List<AudioClip>();
    [SerializeField][Range(0, 1)] float Skill_Range_0_Hit_Volume;
    [SerializeField] List<AudioClip> PlayerHitSound = new List<AudioClip>();
    [SerializeField][Range(0, 1)] float PlayerHitSound_Volume;
    [SerializeField] List<AudioClip> ManaFullSound = new List<AudioClip>();
    [SerializeField][Range(0, 1)] float ManaFullSound_Volume;

    public static PlayerSounds instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            AudioSource = GetComponent<AudioSource>();
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void PlaySound(string name)
    {

        switch (name)
        {
            case "WalkSound_Grass":
                AudioSource.PlayOneShot(WalkSound_Grass[Random.Range(0, WalkSound_Grass.Count)], WalkSound_Grass_Volume);
                break;
            case "DashSound":
                AudioSource.PlayOneShot(DashSound[Random.Range(0, DashSound.Count)], DashSound_Volume);
                break;
            case "RunSound_Grass":
                AudioSource.PlayOneShot(RunSound_Grass[Random.Range(0, RunSound_Grass.Count)], RunSound_Grass_Volume);
                break;
            case "RunStopSound_Grass":
                AudioSource.PlayOneShot(RunStopSound_Grass[Random.Range(0, RunStopSound_Grass.Count)], RunStopSound_Grass_Volume);
                break;
            case "RollSound":
                AudioSource.PlayOneShot(RollSound[Random.Range(0, RollSound.Count)], RollSound_Volume);
                break;
            case "InAirDashSound":
                AudioSource.PlayOneShot(InAirDashSound[Random.Range(0, InAirDashSound.Count)], InAirDashSound_Volume);
                break;
            case "JumpSound_Grass":
                AudioSource.PlayOneShot(JumpSound_Grass[Random.Range(0, JumpSound_Grass.Count)], JumpSound_Grass_Volume);
                break;
            case "WallJumpSound_Grass":
                AudioSource.PlayOneShot(WallJumpSound_Grass[Random.Range(0, WallJumpSound_Grass.Count)], WallJumpSound_Grass_Volume);
                break;
            case "LandSound_Grass":
                AudioSource.PlayOneShot(LandSound_Grass[Random.Range(0, LandSound_Grass.Count)], LandSound_Grass_Volume);
                break;
            case "BaskAttack_Swing":
                AudioSource.PlayOneShot(BaskAttack_Swing[Random.Range(0, BaskAttack_Swing.Count)], BaskAttack_Swing_Volume);
                break;
            case "BaskAttack_EnemyHit_0":
                AudioSource.PlayOneShot(BaskAttack_EnemyHit_0[Random.Range(0, BaskAttack_EnemyHit_0.Count)], BaskAttack_EnemyHit_0_Volume);
                break;

            case "Skill_Range_0_Pre":
                AudioSource.PlayOneShot(Skill_Range_0_Pre[Random.Range(0, Skill_Range_0_Pre.Count)], Skill_Range_0_Pre_Volume);
                break;
            case "Skill_Range_0_Fire":
                AudioSource.PlayOneShot(Skill_Range_0_Fire[Random.Range(0, Skill_Range_0_Fire.Count)], Skill_Range_0_Fire_Volume);
                break;
            case "Skill_Range_0_Hit":
                AudioSource.PlayOneShot(Skill_Range_0_Hit[Random.Range(0, Skill_Range_0_Hit.Count)], Skill_Range_0_Hit_Volume);
                break;

            case "WallSlideSound_Grass":
                AudioSource.PlayOneShot(WallSlideSound_Grass[Random.Range(0, WallSlideSound_Grass.Count)], WallSlideSound_Grass_Volume);
                break;
            case "PlayerHitSound":
                AudioSource.PlayOneShot(PlayerHitSound[Random.Range(0, PlayerHitSound.Count)], PlayerHitSound_Volume);
                break;
            case "ManaFullSound":
                AudioSource.PlayOneShot(ManaFullSound[Random.Range(0, ManaFullSound.Count)], ManaFullSound_Volume);
                break;
        }
    }

    /* public void PlayRepeatSound(string name)
    {
        switch (name)
        {
            case "WallSlideSound_Grass":
                AudioSource.clip = WallSlideSound_Grass;
                AudioSource.loop = true;
                AudioSource.Play();
                AudioSource.PlayOneShot(WallSlideSound_Grass, WallSlideSound_Grass_Volume);
                break;
        }
    }

    public void StopRepeatSound(string name)
    {

    } */
}
