using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Slider masterVolumeSlider;

    private void Start()
    {
        masterVolumeSlider.value = PlayerPrefs.GetFloat("masterVolume", 1.00f);
    }

    public void SetVolume(float sliderValue)
    {
        // name string = Audio Mixer -> Master -> Attenuation -> Volume -> Exposed Parameter
        // dB settings -> -80 and 0, formula: Log(volume) * 20, min value: 0.001 = -80dB max value : 1 = 0 dB
        audioMixer.SetFloat("masterVolume", Mathf.Log10(sliderValue) * 20);
        PlayerPrefs.SetFloat("masterVolume", sliderValue);
    }
}
