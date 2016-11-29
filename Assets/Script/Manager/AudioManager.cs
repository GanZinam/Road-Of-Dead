using UnityEngine;
using System.Collections;

namespace GM
{
    public class AudioManager : MonoBehaviour
    {
        public bool bgSound = true;
        public bool efSound = true;

        [SerializeField]
        AudioClip[] mapSound;
        [SerializeField]
        AudioClip selectStage;
        [SerializeField]
        AudioClip ingameSound;


        [SerializeField]
        AudioSource mainAudio;
        [SerializeField]
        AudioSource serveAudio;
        [SerializeField]
        AudioSource effectAudio;
        [SerializeField]
        AudioSource shotAudio;

        [SerializeField]
        AudioClip effectSound;
        [SerializeField]
        AudioClip shotEff;
        [SerializeField]
        AudioClip reloadEff;

        [SerializeField]
        AudioClip ana_item;
        [SerializeField]
        AudioClip bomb_item;

        [SerializeField]
        AudioClip lionClip;
        [SerializeField]
        AudioClip introBG;

        public void checkMapBG()
        {
            if (PlayerPrefs.GetInt("NowMyPos").Equals(0))
            {
                mainAudio.clip = mapSound[0];
                mainAudio.Play();
            }
            else if (PlayerPrefs.GetInt("NowMyPos").Equals(1))
            {
                mainAudio.clip = mapSound[1];
                mainAudio.Play();
            }
        }

        public void clickBT()
        {
            if (efSound)
                effectAudio.PlayOneShot(effectSound);
        }

        public void selectMap()
        {
            if (bgSound)
            {
                StartCoroutine("decreaseMainVolume");
                serveAudio.clip = selectStage;
                serveAudio.Play();
            }
        }

        public void returnMap()
        {
            if (bgSound)
            {
                StartCoroutine("raiseMainVolume");
            }
        }

        public void startInGame()
        {
            if (bgSound)
            {
                StartCoroutine("changeMainSound");
            }
        }

        IEnumerator changeMainSound()
        {
            while(mainAudio.volume > 0)
            {
                yield return new WaitForSeconds(0.1f);
                mainAudio.volume -= 0.1f;
            }
            mainAudio.clip = ingameSound;
            mainAudio.Play();
            while (mainAudio.volume < 1)
            {
                yield return new WaitForSeconds(0.1f);
                mainAudio.volume += 0.1f;
            }
            StopCoroutine("changeMainSound");
        }

        IEnumerator decreaseMainVolume()
        {
            while (mainAudio.volume >= 0.05f && serveAudio.volume <= 1)
            {
                yield return new WaitForSeconds(0.1f);
                mainAudio.volume -= 0.05f;
                serveAudio.volume += 0.05f;
            }
            StopCoroutine("decreaseMainVolume");
        }

        IEnumerator raiseMainVolume()
        {
            while (mainAudio.volume <= 0.9f && serveAudio.volume >= 0.2f)
            {
                yield return new WaitForSeconds(0.1f);
                mainAudio.volume += 0.05f;
                serveAudio.volume -= 0.05f;
            }
            StopCoroutine("raiseMainVolume");
            serveAudio.Stop();
        }

        IEnumerator decreaseServeVolume()
        {
            while (serveAudio.volume >= 0)
            {
                yield return new WaitForSeconds(0.1f);
                serveAudio.volume -= 0.1f;
            }
            StopCoroutine("decreaseServeVolume");
            serveAudio.Stop();
        }

        IEnumerator raiseServeVolume()
        {
            while (serveAudio.volume<= 1)
            {
                yield return new WaitForSeconds(0.1f);
                serveAudio.volume += 0.01f;
            }
            StopCoroutine("raiseServeVolume");
        }

        public void soundOn(bool b)
        {
            bgSound = b;
            if (!bgSound)
            {
                mainAudio.volume = 0;
                serveAudio.volume = 0;
            }
            else
            {
                mainAudio.volume = 1;
                serveAudio.volume = 1;
            }
        }
        public void effectOn(bool b)
        {
            efSound = b;
            if (!efSound)
            {
                effectAudio.volume = 0;
            }
            else
            {
                effectAudio.volume = 1;
            }
        }

        public void shotEffect()
        {
            shotAudio.volume = 0.5f;
            shotAudio.PlayOneShot(shotEff);
        }
        public void reloadEffect()
        {
            effectAudio.PlayOneShot(reloadEff);
        }


        public void bombEffect()
        {
            effectAudio.Stop();
            effectAudio.volume = 0.4f;
            effectAudio.PlayOneShot(bomb_item);
        }
        public void strongEffect()
        {
            effectAudio.Stop();
            Debug.Log("FF");
            effectAudio.volume = 1;
            effectAudio.PlayOneShot(ana_item);
        }

        public void introEffect()
        {
            effectAudio.Stop();
            effectAudio.volume = 1;
            effectAudio.PlayOneShot(lionClip);
        }
        public void introSound()
        {
            serveAudio.Stop();
            serveAudio.clip = introBG; 
            serveAudio.Play();
            StartCoroutine("raiseServeVolume");
        }
    }
}
