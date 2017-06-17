using UnityEngine;
using System.Collections;
using System.IO;
using UnityEditor;
using System.Collections.Generic;
namespace GJM.Editors
{

    /// <summary>
    /// 获取文件夹下所有文件的路径 保存成配置文件
    /// </summary>
    public class GenerateResConfig : Editor
    {


        #region 生成资源


        [MenuItem("GJM Tools /Resources/生成配表TXT/生成所有")]
        public static void GenerateAll()
        {
            GeneratePrefab();
            GenerateJPG();
            GenerateAnimators();
            GenerateMp3All();
            GenerateFBX();
        }


        [MenuItem("GJM Tools /Resources/生成配表TXT/Prefab")]
        public static void GeneratePrefab()
        {
            string path = Path.Combine(Application.dataPath, "Resources").Replace(@"\", "/");
            string[] resFiles = Directory.GetFiles(path, "*.prefab", SearchOption.AllDirectories);
            for (int i = 0; i < resFiles.Length; i++)
            {
                string filePath = resFiles[i].Replace
                    (@"\", "/").Replace(path + "/", "").Replace(".prefab", "");
                string fileName = Path.GetFileNameWithoutExtension(resFiles[i]);
                resFiles[i] = fileName + "=" + filePath;
            }
            File.WriteAllLines(Path.Combine(path, "ResMapPrefab.txt"), resFiles);
            AssetDatabase.Refresh();
        }


        [MenuItem("GJM Tools /Resources/生成配表TXT/JPG")]
        public static void GenerateJPG()
        {
            string path = Path.Combine(Application.dataPath, "Resources").Replace(@"\", "/");
            string[] resFiles = Directory.GetFiles(path, "*.jpg", SearchOption.AllDirectories);
            for (int i = 0; i < resFiles.Length; i++)
            {
                string filePath = resFiles[i].Replace
                    (@"\", "/").Replace(path + "/", "").Replace(".jpg", "");
                string fileName = Path.GetFileNameWithoutExtension(resFiles[i]);
                resFiles[i] = fileName + "=" + filePath;
            }
            File.WriteAllLines(Path.Combine(path, "ResMapJpg.txt"), resFiles);
            AssetDatabase.Refresh();
        }

        #region Mp3 Tp Res Map
        [MenuItem("GJM Tools /Resources/生成配表TXT/AudioClip/All C.E.CE.EE.S")]
        public static void GenerateMp3All()
        {
            GenerateMP3Chine();
            GenerateMP3English();
            GenerateMP3ChineExplain();
            GenerateMP3EnglishExplain();
            GenerateMP3EnglishSound();
        }


        [MenuItem("GJM Tools /Resources/生成配表TXT/AudioClip/Chine")]
        public static void GenerateMP3Chine()
        {
            List<string> resFileList = new List<string>();
            string path = Path.Combine(Application.dataPath, "Resources").Replace(@"\", "/");
            string[] resFiles = Directory.GetFiles(path, "*.mp3", SearchOption.AllDirectories);
            for (int i = 0; i < resFiles.Length; i++)
            {
                string filePath = resFiles[i].Replace(@"\", "/").Replace(path + "/", "").Replace(".mp3", "");
                string fileName = Path.GetFileNameWithoutExtension(resFiles[i]);
                if (fileName.Split('_').Length > 1)
                {
                    if (fileName.Split('_')[1].Equals("C"))
                        resFileList.Add(fileName + "=" + filePath);
                }
            }
            File.WriteAllLines(Path.Combine(path, "ResMapMp3_Chine.txt"), resFileList.ToArray());
            AssetDatabase.Refresh();
        }

        [MenuItem("GJM Tools /Resources/生成配表TXT/AudioClip/English")]
        public static void GenerateMP3English()
        {

            List<string> resFileList = new List<string>();
            string path = Path.Combine(Application.dataPath, "Resources").Replace(@"\", "/");
            string[] resFiles = Directory.GetFiles(path, "*.mp3", SearchOption.AllDirectories);
            for (int i = 0; i < resFiles.Length; i++)
            {
                string filePath = resFiles[i].Replace(@"\", "/").Replace(path + "/", "").Replace(".mp3", "");
                string fileName = Path.GetFileNameWithoutExtension(resFiles[i]);
                if (fileName.Split('_').Length > 1)
                {
                    if (fileName.Split('_')[1].Equals("E"))
                        resFileList.Add(fileName + "=" + filePath);
                }
            }
            File.WriteAllLines(Path.Combine(path, "ResMapMp3_English.txt"), resFileList.ToArray());
            AssetDatabase.Refresh();
        }

        [MenuItem("GJM Tools /Resources/生成配表TXT/AudioClip/Chine Explain")]
        public static void GenerateMP3ChineExplain()
        {
            List<string> resFileList = new List<string>();
            string path = Path.Combine(Application.dataPath, "Resources").Replace(@"\", "/");
            string[] resFiles = Directory.GetFiles(path, "*.mp3", SearchOption.AllDirectories);
            for (int i = 0; i < resFiles.Length; i++)
            {
                string filePath = resFiles[i].Replace(@"\", "/").Replace(path + "/", "").Replace(".mp3", "");
                string fileName = Path.GetFileNameWithoutExtension(resFiles[i]);
                if (fileName.Split('_').Length > 1)
                {
                    if (fileName.Split('_')[1].Equals("CE"))
                        resFileList.Add(fileName + "=" + filePath);
                }
            }
            File.WriteAllLines(Path.Combine(path, "ResMapMp3_ChineExplain.txt"), resFileList.ToArray());
            AssetDatabase.Refresh();
        }

        [MenuItem("GJM Tools /Resources/生成配表TXT/AudioClip/English Explain")]
        public static void GenerateMP3EnglishExplain()
        {

            List<string> resFileList = new List<string>();
            string path = Path.Combine(Application.dataPath, "Resources").Replace(@"\", "/");
            string[] resFiles = Directory.GetFiles(path, "*.mp3", SearchOption.AllDirectories);
            for (int i = 0; i < resFiles.Length; i++)
            {
                string filePath = resFiles[i].Replace(@"\", "/").Replace(path + "/", "").Replace(".mp3", "");
                string fileName = Path.GetFileNameWithoutExtension(resFiles[i]);
                if (fileName.Split('_').Length > 1)
                {
                    if (fileName.Split('_')[1].Equals("EE"))
                        resFileList.Add(fileName + "=" + filePath);
                }
            }
            File.WriteAllLines(Path.Combine(path, "ResMapMp3_EnglishExplain.txt"), resFileList.ToArray());
            AssetDatabase.Refresh();
        }

        [MenuItem("GJM Tools /Resources/生成配表TXT/AudioClip/Sound")]
        public static void GenerateMP3EnglishSound()
        {

            List<string> resFileList = new List<string>();
            string path = Path.Combine(Application.dataPath, "Resources").Replace(@"\", "/");
            string[] resFiles = Directory.GetFiles(path, "*.mp3", SearchOption.AllDirectories);
            for (int i = 0; i < resFiles.Length; i++)
            {
                string filePath = resFiles[i].Replace(@"\", "/").Replace(path + "/", "").Replace(".mp3", "");
                string fileName = Path.GetFileNameWithoutExtension(resFiles[i]);
                if (fileName.Split('_').Length > 1)
                {
                    if (fileName.Split('_')[1].Equals("S"))
                        resFileList.Add(fileName + "=" + filePath);
                }
            }
            File.WriteAllLines(Path.Combine(path, "ResMapMp3_Sound.txt"), resFileList.ToArray());
            AssetDatabase.Refresh();
        }


        #endregion


        [MenuItem("GJM Tools /Resources/生成配表TXT/Animators")]
        public static void GenerateAnimators()
        {
            string path = Path.Combine(Application.dataPath, "Resources").Replace(@"\", "/");
            string[] resFiles = Directory.GetFiles(path, "*.controller", SearchOption.AllDirectories);
            for (int i = 0; i < resFiles.Length; i++)
            {
                string filePath = resFiles[i].Replace
                    (@"\", "/").Replace(path + "/", "").Replace(".controller", "");
                string fileName = Path.GetFileNameWithoutExtension(resFiles[i]);
                resFiles[i] = fileName + "=" + filePath;
            }
            File.WriteAllLines(Path.Combine(path, "ResMapAnimators.txt"), resFiles);
            AssetDatabase.Refresh();
        }

        [MenuItem("GJM Tools /Resources/生成配表TXT/FBX")]
        public static void GenerateFBX()
        {
            string path = Path.Combine(Application.dataPath, "Resources").Replace(@"\", "/");
            string[] resFiles = Directory.GetFiles(path, "*.FBX", SearchOption.AllDirectories);
            for (int i = 0; i < resFiles.Length; i++)
            {
                string filePath = resFiles[i].Replace
                    (@"\", "/").Replace(path + "/", "").Replace(".FBX", "");
                string fileName = Path.GetFileNameWithoutExtension(resFiles[i]);
                resFiles[i] = fileName + "=" + filePath;
            }
            File.WriteAllLines(Path.Combine(path, "ResMapFBX.txt"), resFiles);
            AssetDatabase.Refresh();
        }

        #endregion


        [MenuItem("GJM Tools /Resources/正式配表TXT/JPG")]
        public static void GenerateOfficialJPG()
        {
            string path = Path.Combine(Application.dataPath, "Resources").Replace(@"\", "/");
            string[] resFiles = Directory.GetFiles(path, "*.jpg", SearchOption.AllDirectories);
            for (int i = 0; i < resFiles.Length; i++)
            {
                string filePath = resFiles[i].Replace
                    (@"\", "/").Replace(path + "/", "").Replace(".jpg", "");
                string fileName = Path.GetFileNameWithoutExtension(resFiles[i]);
                resFiles[i] = fileName + "=" + filePath;
            }
            File.WriteAllLines(Path.Combine(path, "OfficialJpg.txt"), resFiles);
            AssetDatabase.Refresh();
        }


        [MenuItem("GJM Tools /Resources/正式配表TXT/Prefab")]
        public static void GenerateOfficialPrefab()
        {
            string path = Path.Combine(Application.dataPath, "Resources").Replace(@"\", "/");
            string[] resFiles = Directory.GetFiles(path, "*.prefab", SearchOption.AllDirectories);
            for (int i = 0; i < resFiles.Length; i++)
            {
                string filePath = resFiles[i].Replace
                    (@"\", "/").Replace(path + "/", "").Replace(".prefab", "");
                string fileName = Path.GetFileNameWithoutExtension(resFiles[i]);
                resFiles[i] = fileName + "=" + filePath;
            }
            File.WriteAllLines(Path.Combine(path, "OfficialPrefab.txt"), resFiles);
            AssetDatabase.Refresh();
        }

        static int number = 0;
        [MenuItem("GJM Tools /Resources/正式配表TXT/AudioClip")]
        public static void GeneraeOfficiaMP3()
        {

            List<string> c = new List<string>();
            List<string> e = new List<string>();
            List<string> cc = new List<string>();
            List<string> ec = new List<string>();
            List<string> s = new List<string>();


            string path = Path.Combine(Application.dataPath, "Resources").Replace(@"\", "/");
            string[] resFiles = Directory.GetFiles(path, "*.mp3", SearchOption.AllDirectories);
            for (int i = 0; i < resFiles.Length; i++)
            {
                string filePath = resFiles[i].Replace(@"\", "/").Replace(path + "/", "").Replace(".mp3", "");
                string fileName = Path.GetFileNameWithoutExtension(resFiles[i]);
                if (fileName.Split('_').Length > 1)
                {
                    switch (fileName.Split('_')[1])
                    {
                        case "C":
                            c.Add(fileName + "=" + filePath);
                            break;
                        case "CC":
                            cc.Add(fileName + "=" + filePath);
                            break;
                        case "E":
                            e.Add(fileName + "=" + filePath);
                            break;
                        case "EC":
                            ec.Add(fileName + "=" + filePath);
                            break;
                        case "S": 
                            s.Add(fileName + "=" + filePath);
                            break;
                        default:
                            Debug.Log(" default ：" + fileName + "=" + filePath); break;
                    }


                }
            }
            File.WriteAllLines(Path.Combine(path, "Official_C.txt"), c.ToArray());
            File.WriteAllLines(Path.Combine(path, "Official_E.txt"), e.ToArray());
            File.WriteAllLines(Path.Combine(path, "Official_CC.txt"), cc.ToArray());
            File.WriteAllLines(Path.Combine(path, "Official_EC.txt"), ec.ToArray());
            File.WriteAllLines(Path.Combine(path, "Official_S.txt"), s.ToArray());





            c.Clear();
            e.Clear();
            cc.Clear();
            ec.Clear();
            s.Clear();
            AssetDatabase.Refresh();
        }



    }
}