using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Text;
public class matrix3 : MonoBehaviour
{

    public struct stmatrix
    {
        public string color;
        public GameObject go;
    };
    //дані гри
    public Bluetooth ble;

    public stmatrix[,] matrix;

    //відстань в квадратах
    public int countheight;
    public int countwidth;

    public float alpha;
    //основа
    public GameObject root;
    private RectTransform root_transform;
    //дані
    public GameObject prefab;
    public Sprite Red;
    public Sprite White;
    public string colornow = "Red";

    public string action = "add";
    

    public void set_matrix()  //скидання матриці
    {
        matrix = new stmatrix[countwidth, countheight];
        root_transform = root.GetComponent<RectTransform>();
    }

    public void reset_matrix()
    {
        for (int i = 0; i < countwidth; i++)
            for (int j = 0; j < countheight; j++)
            {
                if (matrix[i, j].go != null)
                    Destroy(matrix[i, j].go);
                matrix[i, j].go = null;
                spawn("White", i, j);
            };

    }

    public void set_alpha() //встановлення ширини і висоти трикутника
    {
        float matrix_proportion = (float)countwidth / (float)countheight;
        float image_proportion = root_transform.rect.width / root_transform.rect.height;

        if (image_proportion > matrix_proportion)       
             alpha = root_transform.rect.height / countheight;
        else
            alpha = root_transform.rect.width / countwidth;
    }

    public void set_color(GameObject now, string color)
    {
        Image img = now.GetComponent<Image>();

        if (color == "Red")
            img.sprite = Red;
        if (color == "White")
            img.sprite = White;
    }

    public void spawn(string color, int rx, int ry)
    {
        //з префабу:
        GameObject GO = prefab;
        
        //координати спавна
        Vector3 v = new Vector3(0,0, 0);
        //створення об'єкта
        Instantiate(GO, v, root_transform.rotation, root_transform);
        //конфігурації
        
        GameObject now = GameObject.Find(GO.name + "(Clone)");
        RectTransform rectnow = now.GetComponent<RectTransform>();
        now.name = "pro_triangle";
        rectnow.localPosition = new Vector3((rx - countwidth/2 + 0.5f) * alpha, (ry - countheight/2 +0.5f) * alpha, 0);
        //так просто, генерація позиції
        //далі норм
        //запис в матрицю
        matrix[rx, ry].color = color;
        matrix[rx, ry].go = now;
        //встановлення кординат
        //встановлення розміру трикутника
        rectnow.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, alpha);
        rectnow.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, alpha);
        set_color(now, color);
    }

    public void clear()
    {
        for (int i = 0; i < countwidth; i++)
            for (int j = 0; j < countheight; j++)
            {
                set_color(matrix[i, j].go, "White");
                matrix[i, j].color = "White";
            }
    }
    public void setred()
    {
        colornow = "Red";
    }
    public void setwhite()
    {
        colornow = "White";
    }

    public void set_position(float x, float y)
    {
        Vector3 vc = Camera.main.ScreenToWorldPoint(new Vector3(x, y, 0));
        Vector3 vm;

        float world_alpha = root.transform.TransformPoint((new Vector3(alpha,0,0))).x / 2;


        print("vc:  "+ x + " " + y);
        int i, j;
        for (i = 0; i < countwidth; i++)
            for (j = 0; j < countheight; j++)
            {
                vm = root.transform.TransformPoint((new Vector3((i - countwidth / 2 + 0.5f) * alpha, (j - countheight / 2 + 0.5f) * alpha, 0)));
                float rx = (i - countwidth / 2 + 0.5f) * alpha;
                float ry = (j - countheight/ 2 + 0.5f) * alpha;

                if (i == 0 && j == 0)
                    print("r:  " + rx + " " + ry);

                if (Mathf.Abs(vc.x  - vm.x) < world_alpha && Mathf.Abs(vc.y - vm.y) < world_alpha)
                {
                    if(action == "add")
                    {
                        matrix[i, j].color = colornow;
                        set_color(matrix[i, j].go, colornow);

                    }

                };
            };
    }


    private void Start()
    {
        set_matrix();
        set_alpha();
        reset_matrix();
    }


    public void send()
    {
        int VerticalA = countheight / 8;
        int HorisontalA = countwidth / 8;
        byte[] data = new byte[countheight * HorisontalA];
        for (int i = 0; i < countheight * HorisontalA; i++)
        {
            byte res = new byte();
            int value = new int();
            int tempx = new int();
            int tempy = new int();

            if ((i / countheight + 2) % 2 == 1)
                tempy = i % countheight;
            else
                tempy = countheight - 1 - (i % countheight);

            for (int j = 0; j < 8; j++)
            {
                if ((i / countheight + 2) % 2 == 0)
                    tempx = (i / countheight) * 8 + j;
                else
                    tempx = (i / countheight) * 8 + (7 - j);

                if (matrix[tempx, tempy].color == "Red")
                    value = 1;
                else
                if (matrix[tempx, tempy].color == "White")
                    value = 0;
                res += (byte)(value * Mathf.Pow(2, j));
            };
            data[i] = res;
        };
        ble.SendBytes(data);

    }
}
