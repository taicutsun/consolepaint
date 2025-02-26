flowchart BT
Canvas(Canvas)
Line(Line)
Point(Point)
Program(Program)
Rectangle(Rectangle)
Shape(Shape)

Canvas  -->  Shape 
Line  -->  Canvas 
Line  -..-|>  Shape 
Point  -->  Canvas 
Point  -..-|>  Shape 
Program  -->  Canvas 
Program  -->  Line 
Program  -->  Point 
Program  -->  Rectangle 
Rectangle  -->  Canvas 
Rectangle  -..-|>  Shape 
Shape  -->  Canvas 
