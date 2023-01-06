using UnityEngine;
using UnityEngine.Audio;
using System.Collections;
using System.Collections.Generic;
using Pixelplacement;

public class AudioController : MonoBehaviour
{
    public AudioSource BaseLoopSource
    {
        get
        {
            return loopSources[0];
        }
    }

    public static AudioController Instance { get; private set; }

    public const float GBC_INTERIOR_BGM_LOWEREDVOLUME = 0.35f;
    public const float GBC_INTERIOR_BGM_FULLVOLUME = 0.55f;

    [SerializeField]
    private List<AudioSource> loopSources = default;

    private List<AudioClip> sfx = new List<AudioClip>();
    private List<AudioClip> loops = new List<AudioClip>();

    private List<AudioSource> ActiveSFXSources
    {
        get
        {
            activeSFX.RemoveAll(x => x == null || ReferenceEquals(x, null));
            return activeSFX;
        }
    }
    private List<AudioSource> activeSFX = new List<AudioSource>();

    public bool Fading { get; set; }

    private Dictionary<string, float> limitedFrequencySounds = new Dictionary<string, float>();
    private Dictionary<string, int> lastPlayedSounds = new Dictionary<string, int>();

    private List<AudioMixer> loadedMixers = new List<AudioMixer>();
    private AudioMixerGroup currentSFXMixer = default;

    private const string SOUNDID_REPEAT_DELIMITER = "#";
    private const float DEFAULT_SPATIAL_BLEND = 0.75f;

    private readonly int[] DEFAULT_LOOPSOURCE_INDICES = new int[] { 0 };

    void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        transform.parent = null;
        DontDestroyOnLoad(gameObject);

        foreach (object o in Resources.LoadAll("Audio/SFX"))
        {
            sfx.Add((AudioClip)o);
        }
        foreach (object o in Resources.LoadAll("Audio/Loops"))
        {
            loops.Add((AudioClip)o);
        }
    }

    public AudioSource GetLoopSource(int index)
    {
        return loopSources[index];
    }

    private float GetVolumeFromOptions(int volume, int maxVolume)
    {
        float normalizedValue = volume / (float)maxVolume;
        float adjustedValue = Mathf.Pow(normalizedValue, 0.2f);
        return (1f - adjustedValue) * -80f;
    }

    public AudioSource PlaySound2D(string soundId, float volume = 1f, float skipToTime = 0f, AudioParams.Pitch pitch = null,
        AudioParams.Repetition repetition = null, AudioParams.Randomization randomization = null, AudioParams.Distortion distortion = null, bool looping = false)
    {
        var source = PlaySound3D(soundId, Vector3.zero, volume, skipToTime, pitch, repetition, randomization, distortion, looping);

        if (source != null)
        {
            source.spatialBlend = 0f;
        }

        return source;
    }

    public AudioSource PlaySound3D(string soundId, Vector3 position, float volume = 1f, float skipToTime = 0f, AudioParams.Pitch pitch = null,
        AudioParams.Repetition repetition = null, AudioParams.Randomization randomization = null, AudioParams.Distortion distortion = null, bool looping = false)
    {
        if (repetition != null)
        {
            if (RepetitionIsTooFrequent(soundId, repetition.minRepetitionFrequency, repetition.entryId))
            {
                return null;
            }
        }

        string randomVariationId = soundId;
        if (randomization != null)
        {
            randomVariationId = GetRandomVariationOfSound(soundId, randomization.noRepeating);
        }

        var source = CreateAudioSourceForSound(randomVariationId, position, looping);
        if (source != null)
        {
            source.volume = volume;
            source.time = source.clip.length * skipToTime;

            if (pitch != null)
            {
                source.pitch = pitch.pitch;
            }

            if (distortion != null)
            {
                if (distortion.muffled)
                {
                    MuffleSource(source);
                }
            }
        }

        activeSFX.Add(source);
        return source;
    }

    public void SetAllSoundsPaused(bool paused)
    {
        ActiveSFXSources.ForEach(x =>
        {
            if (paused)
            {
                x.Pause();
            }
            else
            {
                x.UnPause();
            }
        });
    }

    public void FadeSourceVolume(AudioSource source, float volume, float duration, bool obeyTimescale = true)
    {
        Tween.Volume(source, volume, duration, 0f, obeyTimescale: obeyTimescale);
    }

    public AudioClip GetLoopClip(string loopId)
    {
        return loops.Find(x => x.name.ToLowerInvariant() == loopId.ToLowerInvariant());
    }

    public AudioClip GetAudioClip(string soundId)
    {
        return sfx.Find(x => x.name.ToLowerInvariant() == soundId.ToLowerInvariant());
    }

    private AudioSource CreateAudioSourceForSound(string soundId, Vector3 position, bool looping)
    {
        if (!string.IsNullOrEmpty(soundId))
        {
            AudioClip sound = GetAudioClip(soundId);

            if (sound != null)
            {
                return InstantiateAudioObject(sound, position, looping);
            }
        }

        return null;
    }

    private AudioSource InstantiateAudioObject(AudioClip clip, Vector3 pos, bool looping)
    {
        GameObject tempGO = new GameObject("Audio_" + clip.name);
        tempGO.transform.position = pos;

        AudioSource aSource = tempGO.AddComponent<AudioSource>();
        aSource.clip = clip;
        aSource.outputAudioMixerGroup = currentSFXMixer;
        aSource.spatialBlend = DEFAULT_SPATIAL_BLEND;

        aSource.Play();
        if (looping)
        {
            aSource.loop = true;
        }
        else
        {
            Destroy(tempGO, clip.length * 3f);
        }
        return aSource;
    }

    private bool RepetitionIsTooFrequent(string soundId, float frequencyMin, string entrySuffix = "")
    {
        float time = Time.unscaledTime;
        string soundKey = soundId + entrySuffix;

        if (limitedFrequencySounds.ContainsKey(soundKey))
        {
            if (time - frequencyMin > limitedFrequencySounds[soundKey])
            {
                limitedFrequencySounds[soundKey] = time;
                return false;
            }
        }
        else
        {
            limitedFrequencySounds.Add(soundKey, time);
            return false;
        }

        return true;
    }

    private string GetRandomVariationOfSound(string soundPrefix, bool noRepeating)
    {
        string soundId = "";

        if (!string.IsNullOrEmpty(soundPrefix))
        {
            List<AudioClip> variations = sfx.FindAll(x => x != null && x.name.ToLowerInvariant().StartsWith(soundPrefix.ToLowerInvariant() + SOUNDID_REPEAT_DELIMITER));

            if (variations.Count > 0)
            {
                int index = Random.Range(0, variations.Count) + 1;
                if (noRepeating)
                {
                    if (!lastPlayedSounds.ContainsKey(soundPrefix))
                    {
                        lastPlayedSounds.Add(soundPrefix, index);
                    }
                    else
                    {
                        int breakOutCounter = 0;
                        const int BREAK_OUT_THRESHOLD = 100;
                        while (lastPlayedSounds[soundPrefix] == index && breakOutCounter < BREAK_OUT_THRESHOLD)
                        {
                            index = Random.Range(0, variations.Count) + 1;
                            breakOutCounter++;
                        }

                        if (breakOutCounter >= BREAK_OUT_THRESHOLD - 1)
                        {
                            Debug.Log("Broke out of infinite loop! AudioController.PlayRandomSound.");
                        }

                        lastPlayedSounds[soundPrefix] = index;
                    }
                }

                soundId = soundPrefix + SOUNDID_REPEAT_DELIMITER + index;
            }
            else
            {
                soundId = soundPrefix;
            }
        }

        return soundId;
    }

    private void MuffleSource(AudioSource source, float cutoff = 300f)
    {
        var filter = source.gameObject.AddComponent<AudioLowPassFilter>();
        filter.cutoffFrequency = cutoff;
    }

    private void UnMuffleSource(AudioSource source)
    {
        var lowPassFilter = source.GetComponent<AudioLowPassFilter>();
        if (lowPassFilter != null)
        {
            Destroy(lowPassFilter);
        }
    }

    public void MuffleLoop(float cutoff, int loopIndex = 0)
    {
        MuffleSource(loopSources[loopIndex], cutoff);
    }

    public void UnMuffleLoop(int loopIndex = 0)
    {
        UnMuffleSource(loopSources[loopIndex]);
    }

    public void SetLoopTimeNormalized(float normalizedTime, int loopIndex = 0)
    {
        if (loopSources[loopIndex].clip != null)
        {
            loopSources[loopIndex].time = Mathf.Clamp(normalizedTime * loopSources[loopIndex].clip.length, 0f, loopSources[loopIndex].clip.length - 0.1f);
        }
    }

    public void SetLoopPaused(bool paused)
    {
        foreach (AudioSource loopSource in loopSources)
        {
            if (paused)
            {
                loopSource.Pause();
            }
            else
            {
                loopSource.UnPause();
            }
        }
    }

    public void ResumeLoop(float fadeInSpeed = float.MaxValue)
    {
        foreach (AudioSource loopSource in loopSources)
        {
            loopSource.UnPause();

            if (!loopSource.isPlaying)
            {
                loopSource.Play();
            }
        }
    }

    public void RestartLoop(int sourceIndex = 0)
    {
        loopSources[sourceIndex].Stop();
        loopSources[sourceIndex].time = 0f;
        loopSources[sourceIndex].volume = 1f;
        loopSources[sourceIndex].pitch = 1f;
        loopSources[sourceIndex].Play();
    }

    public void StopAllLoops()
    {
        CancelFades();
        foreach (AudioSource loopSource in loopSources)
        {
            loopSource.Stop();
        }
    }

    public void StopLoop(int sourceIndex = 0)
    {
        loopSources[sourceIndex].Stop();
    }

    public void SetLoopAndPlay(string loopName, int sourceIndex = 0, bool looping = true, bool cancelFades = true)
    {
        if (cancelFades)
        {
            CancelFades();
        }
        TrySetLoop(loopName, sourceIndex);
        RestartLoop(sourceIndex);

        loopSources[sourceIndex].loop = looping;
    }

    public void CrossFadeLoop(string loopName, float duration, float volume = 1f, float newLoopStartTime = 0f)
    {
        if ((loopSources[0].clip == null || loopSources[0].clip.name != loopName) && loops.Exists(x => x.name == loopName))
        {
            CancelFades();
            StartCoroutine(CrossFade(loopName, volume, duration, newLoopStartTime));
        }
    }

    public void FadeOutLoop(float fadeDuration, params int[] sourceIndices)
    {
        CancelFades();

        if (sourceIndices == null || sourceIndices.Length == 0)
        {
            sourceIndices = DEFAULT_LOOPSOURCE_INDICES;
        }

        for (int i = 0; i < sourceIndices.Length; i++)
        {
            StartCoroutine(DoFadeToVolume(fadeDuration, 0f, sourceIndices[i]));
        }
    }

    public void FadeInLoop(float fadeDuration, float toVolume, params int[] sourceIndices)
    {
        CancelFades();

        if (sourceIndices == null || sourceIndices.Length == 0)
        {
            sourceIndices = DEFAULT_LOOPSOURCE_INDICES;
        }

        for (int i = 0; i < sourceIndices.Length; i++)
        {
            StartCoroutine(DoFadeToVolume(fadeDuration, toVolume, sourceIndices[i]));
        }
    }

    public void SetLoopVolumeImmediate(float volume, int sourceIndex = 0)
    {
        CancelFades();
        loopSources[sourceIndex].volume = volume;
    }

    public void SetLoopVolume(float volume, float duration, int sourceIndex = 0, bool cancelOtherFades = true)
    {
        if (cancelOtherFades)
        {
            CancelFades();
        }
        StartCoroutine(DoFadeToVolume(duration, volume, sourceIndex));
    }

    private void CancelFades()
    {
        StopAllCoroutines();
        foreach (AudioSource loopSource in loopSources)
        {
            Tween.Cancel(loopSource.GetInstanceID());
        }
        Fading = false;
    }

    private void TrySetLoop(string loopName, int sourceIndex = 0)
    {
        AudioClip loop = GetLoop(loopName);

        if (loop != null)
        {
            loopSources[sourceIndex].clip = loop;
            loopSources[sourceIndex].pitch = 1f;
        }
    }

    private AudioClip GetLoop(string loopName)
    {
        return loops.Find(x => x.name == loopName);
    }

    private IEnumerator DoFadeToVolume(float duration, float volume, int sourceIndex = 0)
    {
        Fading = true;

        Tween.Volume(loopSources[sourceIndex], volume, duration, 0f, Tween.EaseInOut);
        yield return new WaitForSeconds(duration);

        Fading = false;
    }

    // TODO: make this ACTUALLY crossfade...
    private IEnumerator CrossFade(string newLoop, float volume, float duration, float newLoopStartTimeNormalized, int sourceIndex = 0)
    {
        if (loopSources[0].clip != null && loopSources[0].isPlaying)
        {
            StartCoroutine(DoFadeToVolume(duration * 0.5f, 0f, 1)); // HACK: also fade out 2nd loop source here
            yield return DoFadeToVolume(duration * 0.5f, 0f);
        }

        TrySetLoop(newLoop);
        loopSources[0].time = 0f;
        loopSources[0].Play();
        SetLoopTimeNormalized(newLoopStartTimeNormalized);

        yield return DoFadeToVolume(duration * 0.5f, volume);
    }
}