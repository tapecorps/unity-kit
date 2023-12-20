using UnityEngine;
using TapeCorps.Runtime;
using Serializable = System.SerializableAttribute;

namespace TapeCorps.Customs
{
    #region Check Values

    [Serializable]
    public struct CheckInt
    {
        public bool Checked;
        public int Value;
    }

    [Serializable]
    public struct CheckFloat
    {
        public bool Checked;
        public float Value;
    }


    [Serializable]
    public struct CheckBoolean
    {
        public bool Checked;
        public bool Value;
    }


    [Serializable]
    public struct CheckString
    {
        public bool Checked;
        public string Value;
    }

    [Serializable]
    public struct CheckMaterial
    {
        public bool Checked;
        public Material Value;
    }

    [Serializable]
    public struct CheckGameObject
    {
        public bool Checked;
        public GameObject Value;
    }

    [Serializable]
    public struct CheckVector3
    {
        public bool Checked;
        public Vector3 Value;
    }

    [Serializable]
    public struct CheckVector2
    {
        public bool Checked;
        public Vector2 Value;
    }

    [Serializable]
    public struct CheckVector3Int
    {
        public bool Checked;
        public Vector3Int Value;
    }

    [Serializable]
    public struct CheckVector2Int
    {
        public bool Checked;
        public Vector2Int Value;
    }

    #endregion

    [Serializable]
    public struct Chance
    {
        public float Value;

        public bool Roll => Random.value < (Mathf.Clamp(Value, 0f, 100f) / 100f);

        public Chance(float value = 0f)
        {
            this.Value = value;
        }
    }

    public struct Interval
    {
        public float Timeout { get; private set; }
        public float Last { get; private set; }

        public bool Available => !Check();

        public Interval(float timeout, bool start = false)
        {
            Timeout = timeout;
            Last = (start) ? Time.timeSinceLevelLoad + timeout : Time.timeSinceLevelLoad;
        }

        public void ChangeTimeout(float timeout)
        {
            Timeout = timeout;
        }

        public bool Check()
        {
            if (Last < Time.timeSinceLevelLoad)
            {
                Last = Time.timeSinceLevelLoad + Timeout;
                return true;
            }

            return false;
        }
    }
}