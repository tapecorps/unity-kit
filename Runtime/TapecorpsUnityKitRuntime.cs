using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections;
using Object = UnityEngine.Object;

namespace TapeCorps
{

    namespace Runtime{
        public static class GDK
        {

            public static Object GetObjectByName<T>(this List<T> list, string name)
            {
                foreach (T i in list)
                {
                    if ((i as Object).name == name)
                        return (i as Object);
                }

                return null;
            }

            public static bool TryGetObjectByName<T>(this List<T> list, string name, out T o)
            {
                o = default(T);
                foreach (T i in list)
                {
                    if ((i as Object).name == name)
                    {
                        o = i;
                        return true;
                    }
                }

                return false;
            }

            public static List<Type> RemoveEnums(this List<Type> types)
            {
                types.RemoveAll(t => t.IsEnum);
                return types;
            }

            public static T Random<T>(this T[] array)
            {
                return array[UnityEngine.Random.Range(0, array.Length)];
            }

            public static T Random<T>(this List<T> list)
            {
                return list[UnityEngine.Random.Range(0, list.Count)];
            }

            public static Transform FindParentByName(this Transform target, string name)
            {
                if (target.parent != null) 
                    return target.parent.name.Trim() == name.Trim() ?
                        target.parent : 
                        FindParentByName(target.parent, name);
                else return null;
            }

            public static Transform GetChildByName(this Transform parent, string name, bool includeInactive = false)
            {
                Transform[] children = parent.GetComponentsInChildren<Transform>(includeInactive);

                foreach (Transform c in children)
                    if (c.name.Trim() == name.Trim()) return c;

                return null;
            }

            public static bool TryGetChildByName(this Transform parent, string name, out Transform result, bool includeInactive = false)
            {
                Transform[] children = parent.GetComponentsInChildren<Transform>(includeInactive);

                foreach (Transform c in children)
                {
                    if (c.name.Trim() == name.Trim())
                    {
                        result = c;
                        return true;
                    }

                }

                result = null;
                return false;
            }
            public static bool TryGetChildByNameContains(this Transform parent, string name, out Transform result, bool includeInactive = false)
            {
                Transform[] children = parent.GetComponentsInChildren<Transform>(includeInactive);

                foreach (Transform c in children)
                {
                    if (c.name.Trim().Contains(name.Trim()))
                    {
                        result = c;
                        return true;
                    }

                }

                result = null;
                return false;
            }

            public static Transform GetChildByNameContains(this Transform parent, string contains, bool includeInactive = false)
            {
                Transform[] children = parent.GetComponentsInChildren<Transform>(includeInactive);

                foreach (Transform c in children)
                    if (c.name.Contains(contains)) return c;

                Debug.Log(contains + " not found. Total child count: " + children.Length);

                return null;
            }

            public static Transform[] GetChildrenByName(this Transform parent, string name, bool includeInactive = false)
            {
                Transform[] children = parent.GetComponentsInChildren<Transform>(includeInactive);
                List<Transform> found = new List<Transform>();

                foreach (Transform c in children)
                    if (c.name.Trim() == name.Trim()) found.Add(c);

                return found.ToArray();
            }

            public static Transform[] GetChildrenByNameContains(this Transform parent, string contains, bool includeInactive = false)
            {
                Transform[] children = parent.GetComponentsInChildren<Transform>(includeInactive);
                List<Transform> found = new List<Transform>();

                foreach (Transform c in children)
                    if (c.name.Contains(contains)) found.Add(c);

                return found.ToArray();
            }

            public static bool TryGetComponentInParent<T>(this Transform target, out Component component)
            {
                component = null;
                if (target.parent == null)
                    return false;
                else
                {
                    component = target.GetComponentInParent(typeof(T));
                    return component != null;
                }
            }

            public static string Combine(this string[] array)
            {
                string result = "";
                foreach (string a in array)
                    result += $"{a} ";

                return result.TrimEnd();
            }

            public static string Combine(this int[] array)
            {
                string result = "";
                foreach (int a in array)
                    result += $"{a} ";

                return result.TrimEnd();
            }

            public static Queue<T> ToQueue<T>(this List<T> list)
            {
                Queue<T> values = new Queue<T>();

                foreach (T t in list)
                    values.Enqueue(t);

                return values;
            }

            public static T CreateDeepCopy<T>(this T obj)
            {
                using (var ms = new MemoryStream())
                {
                    IFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(ms, obj);
                    ms.Seek(0, SeekOrigin.Begin);
                    return (T)formatter.Deserialize(ms);
                }
            }

            public static T Clone<T>(this T source)
            {
                Type t = source.GetType();
                object result = Activator.CreateInstance(t);

                FieldInfo[] fields = t.GetFields();

                foreach (FieldInfo f in fields)
                    f.SetValue(result, f.GetValue(source));

                return (T)result;
            }

            public static List<T> Shuffle<T>(this List<T> list)
            {
                int n = list.Count;
                System.Random r = new System.Random();
                while (n > 1)
                {
                    n--;
                    int k = r.Next(n + 1);
                    T value = list[k];
                    list[k] = list[n];
                    list[n] = value;
                }

                return list;
            }

            public static Queue<T> Shuffle<T>(this Queue<T> queue)
            {
                List<T> _queue = queue.ToList();
                _queue = _queue.Shuffle();
                return _queue.ToQueue();
            }

            public static T[] Shuffle<T>(this T[] array)
            {
                int n = array.Length;
                List<T> list = array.ToList();
                list = list.Shuffle();
                return list.ToArray();
            }

            public static int GetRandomByRarity(this object[] pool, int[] rarities, bool debug = false)
            {
                if (pool.Length != rarities.Length)
                    return -1;

                int totalRarity = 0;

                foreach (int r in rarities)
                    totalRarity += r;

                int random = UnityEngine.Random.Range(0, totalRarity);

                int targetIndex = 0;
                int last = 0;

                for (int j = 0; j < pool.Length; j++)
                {
                    if (last + rarities[j] > random && random >= last)
                    {
                        targetIndex = j;
                        break;
                    }

                    last += rarities[j];
                }

                return targetIndex;
            }
            public static string AddSpacesBeforeCapitals(this string text)
            {
                string result = "";

                foreach (char c in text)
                {
                    if (c.ToString() == c.ToString().ToUpper())
                        result += " ";
                    result += c;
                }

                return result.Trim();
            }
            public static void Move<T>(this List<T> list, int oldIndex, int newIndex)
            {
                var item = list[oldIndex];
                list.RemoveAt(oldIndex);
                if (newIndex > oldIndex) newIndex--;
                list.Insert(newIndex, item);
            }
            public static void Move<T>(this List<T> list, T item, int newIndex)
            {
                if (item != null)
                {
                    var oldIndex = list.IndexOf(item);
                    if (oldIndex > -1)
                    {
                        list.RemoveAt(oldIndex);
                        if (newIndex > oldIndex) newIndex--;
                        list.Insert(newIndex, item);
                    }
                }
            }
            public static string Print2DArrayAsTable(this string[,] arr)
            {
                int rows = arr.GetLength(0);
                int cols = arr.GetLength(1);

                string table = "2D Array:\n";

                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < cols; j++)
                        table += arr[i, j].ToString().PadLeft(15) + " ";
                    table += "\n";
                }

                return table;
            }

            public static Rect Sum(this Rect rect1, Rect rect2)
            {
                return new Rect(rect1.position + rect2.position, rect1.size + rect2.size);
            }
            public static string LimitStringLength(this object obj, int Length)
            {
                string text = obj.ToString();

                string result = "";

                for (int i = 0; i < text.Length && i < Length; i++)
                    result += text[i];

                return result;
            }


            public static bool Coinflip => UnityEngine.Random.Range(0, 2) == 0;


            public static Vector3 LerpAngle(Vector3 from, Vector3 to, float time)
            {
                return new Vector3(Mathf.LerpAngle(from.x, to.x, time), Mathf.LerpAngle(from.y, to.y, time), Mathf.LerpAngle(from.z, to.z, time));
            }

            public static Vector2 LerpAngle(Vector2 from, Vector2 to, float time)
            {
                return new Vector2(Mathf.LerpAngle(from.x, to.x, time), Mathf.LerpAngle(from.y, to.y, time));
            }
        }

        public static class Shape
        {
            public static Vector2[] GenerateHexagonalPositions(int layerCount, float radius = 1f, float width = 1f)
            {
                List<Vector2> positions = new List<Vector2>();

                float xOffset = width * 3.0f / 2.0f;
                float zOffset = radius * Mathf.Sqrt(3.0f);

                for (int i = 0; i < layerCount; i++)
                {
                    for (int q = -i; q <= i; q++)
                    {
                        for (int r = Mathf.Max(-i, -q - i); r <= Mathf.Min(i, -q + i); r++)
                        {
                            float xPos = q * xOffset;
                            float yPos = r * zOffset + q * zOffset / 2.0f;
                            positions.Add(new Vector2(xPos, yPos));
                        }
                    }
                }

                return positions.ToArray();
            }
        }

        public static class Calculations
        {
            public static int GCD(this int[] numbers)
            {
                return numbers.Aggregate(GCD);
            }

            public static int GCD(int a, int b)
            {
                return b == 0 ? a : GCD(b, a % b);
            }

            public static Vector2Int ConvertWorldToIsometricGridInt(this Vector2 worldPosition, Vector2 tileSize)
            {
                return Vector2Int.RoundToInt(ConvertWorldToIsometricGrid(worldPosition,tileSize)); 
            }

            public static Vector2 ConvertWorldToIsometricGrid(this Vector2 worldPosition, Vector2 tileSize)
            {
                float tileWidth = tileSize.x; 
                float tileHeight = tileSize.y; 

                float x = (worldPosition.x / tileWidth + worldPosition.y / tileHeight) / 2;
                float y = (worldPosition.y / tileHeight - worldPosition.x / tileWidth) / 2;

                return new Vector2(x, y);
            }

            public static float Percentage(float main, float percentage)
            {
                return (percentage / 100f) * main;
            }

            public static float Percentage(string main, float percentage)
            {
                return (percentage / 100f) * float.Parse(main);
            }

            public static float Percentage(string main, string percentage)
            {
                return (float.Parse(percentage) / 100f) * float.Parse(main);
            }

            public static float Percentage(float main, string percentage)
            {
                return (float.Parse(percentage) / 100f) * main;
            }

            public static int PercentageInt(float main, float percentage)
            {
                return Mathf.RoundToInt((percentage / 100f) * main);
            }

            public static int PercentageInt(string main, float percentage)
            {
                return Mathf.RoundToInt((percentage / 100f) * float.Parse(main));
            }

            public static int PercentageInt(string main, string percentage)
            {
                return Mathf.RoundToInt((float.Parse(percentage) / 100f) * float.Parse(main));
            }

            public static int PercentageInt(float main, string percentage)
            {
                return Mathf.RoundToInt((float.Parse(percentage) / 100f) * main);
            }

            public static int TimeVectorToTick(Vector3Int timeVector)
            {
                timeVector.y += timeVector.x * 60;
                timeVector.z += timeVector.y * 60;
                return timeVector.z;
            }
        }

        public static class Geometry
        {
            public static float Angle2D(Vector2 from, Vector2 to)
            {
                return (Mathf.Rad2Deg * (Mathf.Atan2(from.y - to.y, from.x - to.x)) + 180);
            }

            public static Vector2 TangentLimiter(float angle, Vector2 limit)
            {
                Vector2 result = Vector2.zero;
                float x = 0;
                float y = 0;

                float _ang = angle % 360;
                angle = _ang;

                if (_ang < 0)
                    angle = 360 + _ang;

                float a = limit.x;
                float b = limit.y;
                float c = Mathf.Sqrt(limit.x * limit.x + limit.y * limit.y);

                float aAngle = Mathf.Acos(a / c) * Mathf.Rad2Deg;
                float bAngle = 90 - aAngle;

                if (angle >= aAngle && angle < aAngle + bAngle * 2)
                {
                    x = b * Mathf.Tan((bAngle - (angle - aAngle)) * Mathf.Deg2Rad);
                    y = b;
                }
                else if (angle >= aAngle + bAngle * 2 && angle < aAngle * 3 + bAngle * 2)
                {
                    x = -a;
                    y = a * Mathf.Tan((aAngle - (angle - bAngle * 2 - aAngle)) * Mathf.Deg2Rad);
                }
                else if (angle >= aAngle * 3 + bAngle * 2 && angle < aAngle * 3 + bAngle * 4)
                {
                    x = b * Mathf.Tan((-bAngle + (angle - bAngle * 2 - aAngle * 3)) * Mathf.Deg2Rad);
                    y = -b;
                }
                else
                {
                    x = a;
                    y = a * Mathf.Tan(angle * Mathf.Deg2Rad);
                }

                result = new Vector2(x, y);

                return result;
            }


            public static Vector3 SquareToIsometric(Vector3 position, Vector2 xDisplacement, Vector2 yDisplacement)
            {
                return (position.x * xDisplacement) + (position.y * yDisplacement);
            }

            public static float CalculateIsometricAngle(Vector2 isometricScale)
            {
                float a = isometricScale.y / 2;
                float b = isometricScale.x / 2;

                float c = Mathf.Sqrt(a * a + b * b);

                return Mathf.Asin(a / c) * Mathf.Rad2Deg;
            }
        }

        public static class Imaging
        {
            public static Texture2D Resize(Texture2D texture2D, int targetX, int targetY)
            {
                RenderTexture rt = new RenderTexture(targetX, targetY, 24);
                RenderTexture.active = rt;
                Graphics.Blit(texture2D, rt);
                Texture2D result = new Texture2D(targetX, targetY);
                result.ReadPixels(new Rect(0, 0, targetX, targetY), 0, 0);
                result.Apply();
                return result;
            }

            public static Texture2D PickFromTexture2D(Texture2D original, int sizeX, int sizeY, int offsetX = 0, int offsetY = 0)
            {
                Texture2D result = new Texture2D(sizeX, sizeY);

                for (int y = 0; y < sizeY; y++)
                    for (int x = 0; x < sizeX; x++)
                        result.SetPixel(x, y, original.GetPixel(offsetX + x, offsetY + y));

                result.Apply();

                return result;
            }

            public static Texture2D MultiplyAlpha(Texture2D original, float m)
            {
                Texture2D result = new Texture2D(original.width, original.height);

                for (int y = 0; y < result.height; y++)
                {

                    for (int x = 0; x < result.width; x++)
                    {
                        Color c = original.GetPixel(x, y);
                        c.a *= m;
                        result.SetPixel(x, y, c);
                    }
                }

                result.Apply();

                return result;
            }

            public static class CoroutineUtil
            {
                public static IEnumerator WaitForRealSeconds(float time)
                {
                    float start = Time.realtimeSinceStartup;
                    while (Time.realtimeSinceStartup < start + time)
                        yield return null;
                }
            }

            public static Texture2D AddOnTexture2D(Texture2D a, Texture2D b, int offsetX, int offsetY)
            {
                offsetY = -offsetY;
                int width = Mathf.Max(a.width, b.width + Mathf.Abs(offsetX));
                int height = Mathf.Max(a.height, b.height + Mathf.Abs(offsetY));

                Vector2Int aOffset = Vector2Int.zero;

                if (offsetX < 0)
                {
                    aOffset.x = -offsetX;
                    offsetX = 0;
                }

                if (offsetY < 0)
                {
                    aOffset.y = -offsetY;
                    offsetY = 0;
                }

                Texture2D result = new Texture2D(width, height);

                for (int y = 0; y < height; y++)
                    for (int x = 0; x < width; x++)
                        result.SetPixel(x, y, new Color(0, 0, 0, 0));

                for (int y = 0; y < a.height; y++)
                    for (int x = 0; x < a.width; x++)
                        result.SetPixel(x + aOffset.x, y + aOffset.y, a.GetPixel(x, y));

                for (int y = 0; y < b.height; y++)
                {
                    for (int x = 0; x < b.width; x++)
                    {
                        Color bc = b.GetPixel(x, y);
                        Color ac = result.GetPixel(x + offsetX, y + offsetY);
                        result.SetPixel(x + offsetX, y + offsetY, Color.Lerp(ac, bc, bc.a));
                    }
                }

                result.Apply();
                return result;
            }

            public static Texture2D CopyTexture2D(Texture2D original)
            {
                Texture2D result = new Texture2D(original.width, original.height);
                result.SetPixels(original.GetPixels());
                result.Apply();
                return result;
            }
        }

        public class Logic
        {
            public static bool All(bool[] conditions, bool state)
            {
                bool result = true;

                foreach (bool c in conditions)
                {
                    if (c != state)
                    {
                        result = false;
                        break;
                    }
                }

                return result;
            }

            public static bool Between(float value, float a, float b, bool inclusive = false)
            {
                float max = Mathf.Max(a, b);
                float min = Mathf.Min(a, b);

                return inclusive ? (value <= max && min <= value) : (value < max && min < value);
            }

        }
    }

}
