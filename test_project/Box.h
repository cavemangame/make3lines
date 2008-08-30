#ifndef BOX_H_
#define BOX_H_

class Box
{
public:
// constructors
	Box();
	Box(int _x, int _y):x(_x),y(_y),type(NONE_BOX){}
	Box(int _x, int _y, BOX_TYPE _type):x(_x),y(_y),type(_type){}

// states
	POINT GetPosition(){return POINT(x,y);}
	BOX_TYPE GetType(){return type;}

// events
	virtual void Move(int xw, int yw);
	virtual void DoubleClick(){}
	virtual void Destroy(){}

// destructor
	virtual ~Box();
private:
	int x;
	int y;
	BOX_TYPE type;
};

#endif /* BOX_H_ */
