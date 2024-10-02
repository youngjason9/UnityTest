using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoseTools
{
    namespace Utlis
    {
        public static class UtlisClass
        {

            public static TextMesh CreateWorldText(string text, Transform parent = null, Vector3 localPosition = default(Vector3), int fontSize = 40, Color? color = null, TextAnchor textAnchor = TextAnchor.UpperLeft, TextAlignment textAlignment = TextAlignment.Left, int sortingOrder = 5000)
            {
                if (color == null) color = Color.white;
                return CreateWorldText(parent, text, localPosition, fontSize, color.Value, textAnchor, textAlignment, sortingOrder);
            }

            public static TextMesh CreateWorldText(Transform parent, string text, Vector3 localPosition, int fontSize, Color color, TextAnchor textAnchor, TextAlignment textAlignment, int sortingOrder)
            {
                GameObject gameObject = new GameObject("World_Text", typeof(TextMesh));
                Transform transform = gameObject.transform;
                transform.SetParent(parent);
                transform.localPosition = localPosition;
                TextMesh textMesh = gameObject.GetComponent<TextMesh>();
                textMesh.text = text;
                textMesh.anchor = textAnchor;
                textMesh.alignment = textAlignment;
                textMesh.text = text;
                textMesh.fontSize = fontSize;
                textMesh.color = color;
                textMesh.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;
                return textMesh;

            }

            /// <summary>
            /// 判断有一个对象没有获得相应的实例
            /// </summary>
            public static bool IsNull(object obj)
            {
                if(obj == null){
                    Debug.LogError("对象为空!");
                    return true;
                }
                else{
                    return false;
                }
            }

            /// <summary>
            /// 将二维数组转化为一维数组
            /// </summary>
            /// <typeparam name="T">传入的数组类型</typeparam>
            /// <param name="twoDimArray">传入的二维数组</param>
            /// <returns>返回一维数组</returns>
            public static T[] TwoDimToOneDim<T>(T[,] twoDimArray)
            {
                int rows = twoDimArray.GetLength(0);    //列数
                int cols = twoDimArray.GetLength(1);    //行数
                T[] oneDimArray = new T[rows * cols];


                for (int i = 0; i < rows; i++) // 外层循环用行
                {
                    for (int j = 0; j < cols; j++) // 内层循环用列
                    {
                        oneDimArray[i * cols + j] = twoDimArray[i, j]; // 修正乘法用列
                        //Debug.Log("i = " + i + "j = " + j);
                    }
                }

                return oneDimArray;
            }

            /// <summary>
            /// 将一维数组转化为二维数组
            /// </summary>
            /// <typeparam name="T">传入的数组类型</typeparam>
            /// <param name="oneDimArray">传入的一维数组</param>
            /// <param name="rows">行</param>
            /// <param name="cols">列</param>
            /// <returns>返回二维数组</returns>
            /// <exception cref="System.ArgumentException"></exception>
            public static T[,] OneDimToTwoDim<T>(T[] oneDimArray, int rows, int cols)
            {
                if (oneDimArray.Length != rows * cols)
                {
                    throw new System.ArgumentException("一维数组的元素个数与指定的行列数不匹配！"+oneDimArray.Length+"!="+rows*cols);
                }

                T[,] twoDimArray = new T[rows, cols];
                
                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < cols; j++)
                    {
                        twoDimArray[i, j] = oneDimArray[i * cols + j];
                    }
                }

                return twoDimArray;
            }          
        }
    }  
}

