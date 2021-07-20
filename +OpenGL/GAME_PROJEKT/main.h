#ifndef MAIN_H_INCLUDED
#define MAIN_H_INCLUDED

//------------------------------STRUKTURY-------------------------------------------

//Tworzymy typy dla pozycji i kolorów Terrain
typedef struct {

float x,y,z;
}TCell;

typedef struct {
float r,g,b;

}TColor;
//Utworzenie struktury  przeciwników

//Struktura dla textur
typedef struct
{
float u,v;

}TUV;


//Struktura dla objektów
typedef struct
{
    float x,y,z;
    int type;
    float scale;

}TObject;


#define targetCnt 10

struct {
float x,y,z;
BOOL active;
int type;
float scale;

}target[targetCnt];
//BOOL showMask   = FALSE;
//-----------------------------------------------------------------------------------


//Tworzy mape za pomoca dwu wymiarowej tablicy dla pozycji wierzcholkow
// oraz i textur

#define mapW 100
#define mapH 100

TCell map[mapW][mapH];
TCell mapNormal[mapW][mapH];
TUV mapUV[mapW][mapH];

//Tablica indeksow
GLuint mapInd [mapW-1][mapH-1][6];
int mapIndCnt = sizeof(mapInd) / sizeof(GLuint);


 float plant[] = {-0.5,0,0, 0.5,0,0, 0.5,0,1, -0.5,0,1,
                  0,-0.5,0, 0,0.5,0, 0,0.5,1, 0,-0.5,1};
float plantUV[] = {0,1, 1,1, 1,0, 0,0, 0,1, 1,1, 1,0, 0,0};
GLuint plantInd[] = {0,1,2, 2,3,0, 4,5,6, 6,7,4};

int plantIndCnt = sizeof(plantInd)/sizeof(GLuint);

int tex_pole,tex_trava,tex_tree,tex_tree2,tex_flower;

//tworzenie tablicy objektów
    TObject * plantMas=NULL;
    int plantCnt = 0;

#endif // MAIN_H_INCLUDED
