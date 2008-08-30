#ifndef UTILS_H_
#define UTILS_H_

#define MAX_FIELD_SIZE 16
typedef enum
{
	NONE_BOX,
	BLUE_BOX,
	RED_BOX,
	GREEN_BOX,
	YELLOW_BOX,
	BOMB,
}BOX_TYPE;

typedef struct POINT_
{
	int x;
	int y;
	struct POINT_(int _x, int _y):x(_x), y(_y){}
}POINT;

#endif /* UTILS_H_ */
