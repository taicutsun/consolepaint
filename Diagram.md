flowchart BT
AddShapeAction(AddShapeAction)
Canvas(Canvas)
Ellipse(Ellipse)
FileManager(FileManager)
FillShapeAction(FillShapeAction)
ICanvas(ICanvas)
IUndoableAction(IUndoableAction)
Line(Line)
MoveShapeAction(MoveShapeAction)
Pixel(Pixel)
Point(Point)
Rectangle(Rectangle)
RemoveShapeAction(RemoveShapeAction)
Shape(Shape)
ShapeFactory(ShapeFactory)
ShapeWrapper(ShapeWrapper)
Terminal(Terminal)
Triangle(Triangle)
UndoManager(UndoManager)

AddShapeAction  -->  ICanvas 
AddShapeAction  --*  ICanvas 
AddShapeAction  -..-|>  IUndoableAction 
AddShapeAction  --*  Shape 
Canvas  -..-|>  ICanvas 
Canvas  -..->  Pixel 
Canvas  -->  Pixel 
Canvas  -->  Pixel 
Canvas  -->  Shape 
Ellipse  -->  Pixel 
Ellipse  -..-|>  Shape 
Ellipse  -->  Shape 
FileManager  -->  Shape 
FileManager  -->  ShapeWrapper 
FillShapeAction  -..->  ICanvas 
FillShapeAction  -->  ICanvas 
FillShapeAction  --*  ICanvas 
FillShapeAction  -..-|>  IUndoableAction 
FillShapeAction  -->  Pixel 
FillShapeAction  --*  Shape 
FillShapeAction  -->  Shape 
FillShapeAction  -..->  Shape 
ICanvas  -->  Pixel 
ICanvas  -->  Pixel 
ICanvas  -->  Shape 
Line  -->  Pixel 
Line  -->  Shape 
Line  -..-|>  Shape 
MoveShapeAction  -->  ICanvas 
MoveShapeAction  --*  ICanvas 
MoveShapeAction  -..-|>  IUndoableAction 
MoveShapeAction  --*  Shape 
MoveShapeAction  -->  Shape 
Point  -->  Pixel 
Point  -..-|>  Shape 
Point  -->  Shape 
Rectangle  -->  Pixel 
Rectangle  -->  Shape 
Rectangle  -..-|>  Shape 
RemoveShapeAction  -->  ICanvas 
RemoveShapeAction  --*  ICanvas 
RemoveShapeAction  -..-|>  IUndoableAction 
RemoveShapeAction  --*  Shape 
Shape  -->  Pixel 
ShapeFactory  -->  Ellipse 
ShapeFactory  -->  Line 
ShapeFactory  -->  Point 
ShapeFactory  -->  Rectangle 
ShapeFactory  -->  Shape 
ShapeFactory  -->  Shape 
ShapeFactory  -->  Triangle 
Terminal  -->  AddShapeAction 
Terminal  -->  Canvas 
Terminal  -..->  Canvas 
Terminal  -->  FileManager 
Terminal  -->  FillShapeAction 
Terminal  -->  MoveShapeAction 
Terminal  -->  Pixel 
Terminal  -->  RemoveShapeAction 
Terminal  -->  Shape 
Terminal  -->  Shape 
Terminal  -..->  Shape 
Terminal  -->  ShapeFactory 
Terminal  -..->  UndoManager 
Terminal  -->  UndoManager 
Triangle  -->  Pixel 
Triangle  -..-|>  Shape 
Triangle  -->  Shape 
UndoManager  -->  IUndoableAction 
