using UnityEngine;
using System.Collections.Generic;
 
public class AudioManager : MonoBehaviour {
    public enum SoundType {

        //---Player---//

        Select,
        Command,

        //Units
        WorkerSpawn,
        WorkerDeath,
        WorkerConstruct,
        WorkerChop,
        SoliderSpawn,
        SoliderDeath,
        SoliderAttack,
        WizardSpawn,
        WizardDeath,
        WizardAttack,
        WizardHit,
        JuggernautSpawn,
        JuggernautDeath,
        JuggernautAttack,
        DrakeSpawn,
        DrakeDeath,
        DrakeAttack,

        //Resources and Buildings
        BuildingConstruct,
        BuildingComplete,
        LumberAdded,
        LumberRemoved,
        MetalAdded,
        MetalRemoved,

        //Purchase
        PurchaseSuccess,
        PurchaseFail,

        //---Enemies---//

        //Units
        LizardWarriorSpawn,
        LizardWarriorDeath,
        LizardWarriorAttack,
        LizardWarriorHit,
        DogSpawn,
        DogDeath,
        DogAttack,
        DogHit,

        //---Music---//

        Music_Menu,
        Music_Battle,
        Music_Preparation
        // Add more sound types as needed
    }
 
    [System.Serializable]
    public class Sound {
        public SoundType Type;
        public AudioClip Clip;
 
        [Range(0f, 1f)]
        public float Volume = 1f;
 
        [HideInInspector]
        public AudioSource Source;
    }
 
    //Singleton
    public static AudioManager Instance;
 
    //All sounds and their associated type - Set these in the inspector
    public Sound[] AllSounds;
 
    //Runtime collections
    private Dictionary<SoundType, Sound> _soundDictionary = new Dictionary<SoundType, Sound>();
    private AudioSource _musicSource;
 
    private void Awake() {
        //Assign singleton
        Instance = this;
 
        //Set up sounds
        foreach(var s in AllSounds)
        {
            _soundDictionary[s.Type] = s;
        }

    }
 
 
 
    //Call this method to play a sound
    public void Play(SoundType type) {
        //Make sure there's a sound assigned to your specified type
        if (!_soundDictionary.TryGetValue(type, out Sound s))
        {
            Debug.LogWarning($"Sound type {type} not found!");
            return;
        }

        //Creates a new sound object
        var soundObj = new GameObject($"Sound_{type}");
        var audioSrc = soundObj.AddComponent<AudioSource>();
 
        //Assigns your sound properties
        audioSrc.clip = s.Clip;
        audioSrc.volume = s.Volume;

        //Play the sound
        audioSrc.Play();

        //Destroy the sound object after the clip has finished playing
        Destroy(soundObj, s.Clip.length);
 
    }
 
    //Call this method to change music tracks
    public void ChangeMusic(SoundType type)
    {
        if (!_soundDictionary.TryGetValue(type, out Sound track))
        {
            Debug.LogWarning($"Music track {type} not found!");
            return;
        }
 
        if (_musicSource == null)
        {
            var container = new GameObject("SoundTrackObj");
            _musicSource = container.AddComponent<AudioSource>();
            _musicSource.loop = true;
        }
 
        _musicSource.clip = track.Clip;
        _musicSource.Play();
    }
}
