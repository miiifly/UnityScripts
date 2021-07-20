#include <windows.h>
#include <gl/gl.h>
#include <math.h>
#include "camera.h"
#define STB_IMAGE_IMPLEMENTATION
#include "stb_image.h"
#include "main.h"
#include <mmsystem.h>
LRESULT CALLBACK WindowProc(HWND, UINT, WPARAM, LPARAM);
void EnableOpenGL(HWND hwnd, HDC*, HGLRC*);
void DisableOpenGL(HWND, HDC, HGLRC);
 HWND hwnd;

//------------------------------Przeciwnik------------------------------------------
float kube[] = {0,0,0, 0,1,0, 1,1,0, 1,0,0, 0,0,1, 0,1,1, 1,1,1, 1,0,1};
GLuint kubeInd[] = {0,1,2, 2,3,0, 4,5,6, 6,7,4, 3,2,5, 6,7,3, 0,1,5, 5,4,0,
                    1,2,6, 6,5,1, 0,3,7, 7,4,0};


int killenemy = 0;
//----------------------------------------------------------------------------------

//------------------------------Otrzymanie pliku textur--------------------------
void LoadTexture(char *file_name,int *target)
{
    int width,height,cnt;
    unsigned char *data = stbi_load(file_name,&width,&height,&cnt,0);
    glGenTextures(1,target);
    glBindTexture(GL_TEXTURE_2D,*target);
        glTexParameteri(GL_TEXTURE_2D,GL_TEXTURE_WRAP_S,GL_REPEAT);
        glTexParameteri(GL_TEXTURE_2D,GL_TEXTURE_WRAP_T,GL_REPEAT);
        glTexParameteri(GL_TEXTURE_2D,GL_TEXTURE_MIN_FILTER,GL_NEAREST);
        glTexParameteri(GL_TEXTURE_2D,GL_TEXTURE_MAG_FILTER,GL_NEAREST);
        glTexImage2D(GL_TEXTURE_2D,0,GL_RGBA,width,height, 0 ,cnt==4 ? GL_RGBA : GL_RGB, GL_UNSIGNED_BYTE,data);
   glBindTexture(GL_TEXTURE_2D,0);
   stbi_image_free(data);



}


//-----------------------------GENERACJA GOR-------------------------------
BOOL IsCoordInMap(float x, float y)
{
    return (x>=0) && ( x<mapW) && (y>=0) && (y<mapH);
}
//funkcja tworzenia gor
void Map_CreateHill(int posX, int posY, int rad, int height)
{
    for(int i = posX-rad; i <= posX+rad ;i++)
        for(int j = posY-rad; j<=posY+rad; j++)
            if(IsCoordInMap(i,j))
            {
                float len = sqrt (pow(posX-i,2)+ pow (posY-j,2));
                if(len<rad)
                {
                    len=len/rad*M_PI_2;
                    map[i][j].z += cos(len) * height;
                }


            }
}
//------------------------------------------------------------------------


//--------------------Obliczenia dla otrzymania mapy normali-----------------------------
#define sqr(a) (a)*(a)
//Obliczenie mapy normali
void CalcNormals(TCell a,TCell b,TCell c , TCell *n)
{
    float wrki;
    TCell v1,v2;

    v1.x=a.x-b.x;
    v1.y=a.y-b.y;
    v1.z=a.z-b.z;

    v2.x=b.x-c.x;
    v2.y=b.y-c.y;
    v2.z=b.z-c.z;

    n->x = (v1.y * v2.z - v1.z * v2.y);
    n->y = (v1.z * v2.x - v1.x * v2.z);
    n->z = (v1.x * v2.y - v1.y * v2.x);
    wrki = sqrt(sqr(n->x)+sqr(n->y)+sqr(n->z));
    n->x /=wrki;
    n->y /=wrki;
    n->z /=wrki;

}
//------------------------------------------------------------------------------




//Obliczenie polozenia  (dla gracza oraza dla romieszczenia objektow)
float Map_GetHeight(float x , float y)
{
    if(!IsCoordInMap(x,y)) return 0;
    int cX = (int)x;
    int cY = (int)y;
    //Obliczenie wysokosci w pozycji x pomiedzy dwoch dolnych wierzcholkach
    float h1 = ((1-(x-cX))*map[cX][cY].z   + (x-cX)*map[cX+1][cY].z );
    //Oraz w taki sam sposob dla gornych
    float h2 = ((1-(x-cX))*map[cX][cY+1].z   + (x-cX)*map[cX+1][cY+1].z );
    //Zwracam polozenie
    return (1-(y-cY))*h1 + (y-cY)*h2;
}


void Map_Init()
{   LoadTexture("TEXTURES/pole.png",&tex_pole);
    LoadTexture("TEXTURES/trava.png",&tex_trava);
    LoadTexture("TEXTURES/tree.png",&tex_tree);
    LoadTexture("TEXTURES/tree2.png",&tex_tree2);
     LoadTexture("TEXTURES/flower.png",&tex_flower);




      glEnable(GL_LIGHTING);
    glEnable(GL_LIGHT0);
    glEnable(GL_COLOR_MATERIAL);
    glEnable(GL_NORMALIZE);

    //Przezroczytosc objektów gdy są oni .png
    glEnable(GL_ALPHA_TEST);
    glAlphaFunc(GL_GREATER,0.99);


    for (int i=0; i<mapW; i++)
        for(int j=0;j<mapH;j++)
        {



            map[i][j].x =i;
            map[i][j].y =j;
            // Za pomoca rand w z robie reljef
            map[i][j].z =(rand()%10)*0.05;



            mapUV[i][j].u=i;
            mapUV[i][j].v=j;


        }

        for(int i=0;i<mapW-1;i++)
        {
            int pos = i*mapH;
            for(int j=0;j<mapH-1;j++)
            {
                mapInd[i][j][0]=pos;
                mapInd[i][j][1]=pos+1;
                mapInd[i][j][2]=pos+1+mapH;

                mapInd[i][j][3]=pos+1+mapH;
                mapInd[i][j][4]=pos+mapH;
                mapInd[i][j][5]=pos;

                pos++;
            }
        }
        //Wywolanie funkcji generacji gor
        for (int i=0;i<10;i++){
                Map_CreateHill(rand()%mapW,rand()%mapH,rand()%50,rand()%10);

        }
        //generacja swiatla zapomoca mapy normali
        for(int i=0;i<mapW-1;i++)
            for(int j=0;j< mapH-1;j++)
            {
                CalcNormals(map[i][j],map[i+1][j],map[i][j+1],&mapNormal[i][j]);
            }
//robie to zeby podczas random elemnty umiejscili sie w przedziale mapy
        int w = mapW-2;
        int h = mapH-2;
        int travaN = 2000;
        int treeN = 40;

        plantCnt=travaN+treeN;
        plantMas = realloc(plantMas,sizeof(*plantMas) * plantCnt);

//Losowe rozmieszczenie elemntów
        for(int i=0;i<plantCnt;i++)
        {
            if(i<travaN)
            {
                plantMas[i].type=tex_trava;
                plantMas[i].scale=0.7+(rand()%5)*0.1;
            }
            else
            {
                plantMas[i].type = rand()%2 ==0 ? tex_tree : tex_tree2;
                plantMas[i].scale=4+(rand()%14);
            }
            plantMas[i].x=(rand()% w )+ 1;
            plantMas[i].y=(rand()% h )+ 1;
            plantMas[i].z=Map_GetHeight(plantMas[i].x,plantMas[i].y);
        }
}

//Rozmieszczenie elementów na mapie
void Target_Init()
{    int a = mapW-2;
     int b = mapH-2;

    for(int i=0;i<targetCnt;i++)
    {
            target[i].active = TRUE;
            target[i].type=tex_flower;
            target[i].scale = 1 ;
            target[i].x = 1+(rand()%a);
            target[i].y = 1+(rand()%b);
            target[i].z = Map_GetHeight(target[i].x,target[i].y);

    }
}

//Funkcja poruszania sie gracza
void Player_Move()
{//Gdy zostala nicisniety klawisz W zwracamy do funkcji (1), jesli S to (-1) , gdy nic to 0
    // Tak samo i dla D i A , w koncu wpisujemy predkosc.

    Camera_MoveDirection( GetKeyState('W')< 0 ? 1 : (GetKeyState('S') < 0 ? -1:0)
                         ,GetKeyState('D')< 0 ? 1 : (GetKeyState('A') < 0 ? -1:0)
                         ,0.05);
    for(int i=0;i<targetCnt;i++){
            if(camera.x<=target[i].x+1 && camera.x>=target[i].x-1 && camera.y<=target[i].y+1 && camera.y>=target[i].y-1){
                target[i].active=FALSE;
                target[i].x=-1;
                target[i].y=-1;
                target[i].z=-1;
                killenemy++;

            }

    if(killenemy==targetCnt) PostQuitMessage(0);

    }

    if(camera.x<0 || camera.x>100 || camera.y<0 ||camera.y>100) {
        camera.x = 50;
        camera.y = 50;
    }
    Camera_AutoMoveByMouse(400,400,0.2);
    //         Wysokosc w zadanym punkcie   + wysokosc kamery
    camera.z = Map_GetHeight(camera.x,camera.y)+1.7;
}



//Wyswietlenie przeciwników   potem zostaje wywolane w funkcji Map Show
void Target_Show()
{


        glEnableClientState(GL_VERTEX_ARRAY);
        glEnableClientState(GL_TEXTURE_COORD_ARRAY);

            glVertexPointer(3,GL_FLOAT,0,plant);
            glTexCoordPointer(2,GL_FLOAT,0,plantUV);
            glColor3f(0.7,0.7,0.7);
            glNormal3f(0,0,1);


            for(int i=0;i<targetCnt;i++)
            {

            if(!target[i].active) continue;
            glBindTexture(GL_TEXTURE_2D,target[i].type);
            glPushMatrix();
                glTranslatef(target[i].x,target[i].y,target[i].z);
                glScalef(target[i].scale,target[i].scale,target[i].scale);
                glDrawElements(GL_TRIANGLES,plantIndCnt,GL_UNSIGNED_INT,plantInd);
            glPopMatrix();
            }
        glDisableClientState(GL_VERTEX_ARRAY);
        glDisableClientState(GL_TEXTURE_COORD_ARRAY);
}

//Wyswietlenie Mapy
void Map_Show()
{

    glClearColor(0.6f, 0.8f, 1.0f, 0.0f);
    glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);



    glEnable(GL_TEXTURE_2D);


    glPushMatrix();
        Camera_Apply();
        //Pozycja swiatla
        GLfloat position[] = {1,0,2,0};
        glLightfv(GL_LIGHT0,GL_POSITION,position);
        //Rysowanie swiata gry
        glEnableClientState(GL_VERTEX_ARRAY);
        glEnableClientState(GL_TEXTURE_COORD_ARRAY);
        glEnableClientState(GL_NORMAL_ARRAY);
            glVertexPointer(3,GL_FLOAT,0,map);

            glTexCoordPointer(2,GL_FLOAT,0,mapUV);
            glColor3f(0.7,0.7,0.7);
            glNormalPointer(GL_FLOAT,0,mapNormal);
            glBindTexture(GL_TEXTURE_2D,tex_pole);
            glDrawElements(GL_TRIANGLES,mapIndCnt,GL_UNSIGNED_INT,mapInd);
        glDisableClientState(GL_VERTEX_ARRAY);
        glDisableClientState(GL_TEXTURE_COORD_ARRAY);
        glDisableClientState(GL_NORMAL_ARRAY);



       glEnableClientState(GL_VERTEX_ARRAY);
        glEnableClientState(GL_TEXTURE_COORD_ARRAY);

            glVertexPointer(3,GL_FLOAT,0,plant);
            glTexCoordPointer(2,GL_FLOAT,0,plantUV);
            glColor3f(0.7,0.7,0.7);
            glNormal3f(0,0,1);


            for(int i=0;i<plantCnt;i++)
            {


            glBindTexture(GL_TEXTURE_2D,plantMas[i].type);
            glPushMatrix();
                glTranslatef(plantMas[i].x,plantMas[i].y,plantMas[i].z);
                glScalef(plantMas[i].scale,plantMas[i].scale,plantMas[i].scale);
                glDrawElements(GL_TRIANGLES,plantIndCnt,GL_UNSIGNED_INT,plantInd);
            glPopMatrix();
            }
        glDisableClientState(GL_VERTEX_ARRAY);
        glDisableClientState(GL_TEXTURE_COORD_ARRAY);



        Target_Show();

    glPopMatrix();
}

/*void Player_Shoot()
{
    showMask = TRUE;
    Map_Show();
    showMask = FALSE;

    RECT rct;
    GLubyte clr[3];
    GetClientRect(hwnd,&rct);
    glReadPixels(   rct.right/2.0,rct.bottom /2.0, 2,2,
                    GL_RGB,GL_UNSIGNED_BYTE,clr);
    if(clr[0]>0)
        enemy[255-clr[0]].active = FALSE;
}
*/



//Funkcja zmiany ekranu
void WndResize(int x , int y)
{
    glViewport(0,0,x,y);
    float k=x/(float)y;
    float sz = 0.1;

    glMatrixMode(GL_PROJECTION);

    glLoadIdentity();
    //Projekcja w perspektywie
    glFrustum(-k*sz,k*sz,-sz,sz,sz*2,1000);
    glMatrixMode(GL_MODELVIEW);
    glLoadIdentity();
}

int WINAPI WinMain(HINSTANCE hInstance,
                   HINSTANCE hPrevInstance,
                   LPSTR lpCmdLine,
                   int nCmdShow)
{
    WNDCLASSEX wcex;

    HDC hDC;
    HGLRC hRC;
    MSG msg;
    BOOL bQuit = FALSE;
    float theta = 0.0f;

    /* register window class */
    wcex.cbSize = sizeof(WNDCLASSEX);
    wcex.style = CS_OWNDC;
    wcex.lpfnWndProc = WindowProc;
    wcex.cbClsExtra = 0;
    wcex.cbWndExtra = 0;
    wcex.hInstance = hInstance;
    wcex.hIcon = LoadIcon(NULL, IDI_APPLICATION);
    wcex.hCursor = LoadCursor(NULL, IDC_ARROW);
    wcex.hbrBackground = (HBRUSH)GetStockObject(BLACK_BRUSH);
    wcex.lpszMenuName = NULL;
    wcex.lpszClassName = "GLSample";
    wcex.hIconSm = LoadIcon(NULL, IDI_APPLICATION);;


    if (!RegisterClassEx(&wcex))
        return 0;

    /* create main window */
    hwnd = CreateWindowEx(0,
                          "GLSample",
                          "OpenGL Sample",
                          WS_OVERLAPPEDWINDOW,
                          CW_USEDEFAULT,
                          CW_USEDEFAULT,
                          1100,
                          700,
                          NULL,
                          NULL,
                          hInstance,
                          NULL);

    ShowWindow(hwnd, nCmdShow);

    /* enable OpenGL for the window */
    EnableOpenGL(hwnd, &hDC, &hRC);


    //WYWOLANIE FUKCJI ZMIANY EKRANU
    RECT rct;
    GetClientRect(hwnd,&rct);
    WndResize(rct.right,rct.bottom);
    Map_Init();
    Target_Init();
    glEnable(GL_DEPTH_TEST);



    /* program main loop */
    while (!bQuit)
    {
        /* check for messages */
        if (PeekMessage(&msg, NULL, 0, 0, PM_REMOVE))
        {
            /* handle or dispatch messages */
            if (msg.message == WM_QUIT)
            {
                bQuit = TRUE;
            }
            else
            {
                TranslateMessage(&msg);
                DispatchMessage(&msg);
            }
        }
        else
        {
            /* OpenGL animation code goes here */
            if(GetForegroundWindow()==hwnd)
                Player_Move();
            //Utworzenie Kamery

            Map_Show();
            SwapBuffers(hDC);

            theta += 1.0f;
            Sleep (1);
        }
    }

    /* shutdown OpenGL */
    DisableOpenGL(hwnd, hDC, hRC);

    /* destroy the window explicitly */
    DestroyWindow(hwnd);

    return msg.wParam;
}

LRESULT CALLBACK WindowProc(HWND hwnd, UINT uMsg, WPARAM wParam, LPARAM lParam)
{
    switch (uMsg)
    {
        case WM_CLOSE:
            PostQuitMessage(0);
        break;


        case WM_SIZE:
             WndResize(LOWORD(lParam),LOWORD(lParam));
        break;

        case WM_SETCURSOR:
            ShowCursor(FALSE);

        break;

        case    WM_LBUTTONDOWN:
           // Player_Shoot();
        break;


        case WM_DESTROY:
            return 0;

        case WM_KEYDOWN:
        {
            switch (wParam)
            {
                case VK_ESCAPE:
                    PostQuitMessage(0);
                break;
            }
        }
        break;

        default:
            return DefWindowProc(hwnd, uMsg, wParam, lParam);
    }

    return 0;
}

void EnableOpenGL(HWND hwnd, HDC* hDC, HGLRC* hRC)
{
    PIXELFORMATDESCRIPTOR pfd;

    int iFormat;

    /* get the device context (DC) */
    *hDC = GetDC(hwnd);

    /* set the pixel format for the DC */
    ZeroMemory(&pfd, sizeof(pfd));

    pfd.nSize = sizeof(pfd);
    pfd.nVersion = 1;
    pfd.dwFlags = PFD_DRAW_TO_WINDOW |
                  PFD_SUPPORT_OPENGL | PFD_DOUBLEBUFFER;
    pfd.iPixelType = PFD_TYPE_RGBA;
    pfd.cColorBits = 24;
    pfd.cDepthBits = 16;
    pfd.iLayerType = PFD_MAIN_PLANE;

    iFormat = ChoosePixelFormat(*hDC, &pfd);

    SetPixelFormat(*hDC, iFormat, &pfd);

    /* create and enable the render context (RC) */
    *hRC = wglCreateContext(*hDC);

    wglMakeCurrent(*hDC, *hRC);
}

void DisableOpenGL (HWND hwnd, HDC hDC, HGLRC hRC)
{
    wglMakeCurrent(NULL, NULL);
    wglDeleteContext(hRC);
    ReleaseDC(hwnd, hDC);
}

