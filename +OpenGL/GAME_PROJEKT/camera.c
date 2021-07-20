
#include <GL/gl.h>
#include "camera.h"
#include <windows.h>
#include <math.h>
struct SCamera camera = {50,50,1.7,0,0};

void Camera_Apply()
{

    glRotatef(-camera.Xrot, 1,0,0);
    glRotatef(-camera.Zrot, 0,0,1);
    glTranslatef(-camera.x,-camera.y,-camera.z);
}
//FUNKCJA POWRACANIA KAMERY
void Camera_Rotation(float xAngle, float zAngle)
{

    camera.Zrot +=zAngle;
    if(camera.Zrot < 0) camera.Zrot +=360;
    if(camera.Zrot > 360) camera.Zrot -=360;
    camera.Xrot += xAngle;
    if(camera.Xrot < 0) camera.Xrot = 0;
    if(camera.Xrot >180) camera.Xrot = 180;
}

//Obrut Kamery za pomoca myszky
//DLA (POINT oraz GetCursor,SetCursor) jest potrzebne podlaczenie <windows.h>
void Camera_AutoMoveByMouse (int centerX , int centerY , float speed)
{
    POINT cur;
    //Zatrzymuje musz w zadanym punkcie
    POINT base = {centerX,centerY};
    GetCursorPos(&cur);
    Camera_Rotation( (base.y - cur.y)*speed , (base.x-cur.x)*speed);
    SetCursorPos(base.x,base.y);

}
//Funkcja poruszania sie w kierunku kamery
void Camera_MoveDirection (int forwardMove , int rightMove,float speed)
{   //Kat pod kturym patrzy sie gracz przeprowadzamy w radiany
    float kat = -camera.Zrot / 180* M_PI;
    //rightMove - odpowiada zaporuszenie sie w prawo w lewo
    //W zaleznosci w jaka strone gracz sie porusza zmieniamy kat
    //Do Przodu
    if(forwardMove>0)
        kat +=rightMove>0 ? M_PI_4 : (rightMove<0 ? -M_PI_4:0);
    //Do tylu
    if(forwardMove<0)
        kat += M_PI + (rightMove > 0 ? -M_PI_4 : (rightMove < 0 ? M_PI_4 : 0));
    //Stoimy na miejscu
    if(forwardMove == 0)
     {
         kat += rightMove > 0 ? M_PI_2 : -M_PI_2;
         if(rightMove == 0) speed = 0;

     }
    //Predkosc poruszania sie
    if(speed !=0)
    {
        camera.x +=sin(kat)*speed;
        camera.y +=cos(kat)*speed;
    }
}
