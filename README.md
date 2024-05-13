# 3D graphics
This is a project created for Computer Graphics course at Warsaw University of Technology (faculty of Mathematics and Infotmation Science) at 5th semester (2022-2023). Part of the project was created at hom and part of the project was done during laboratories in limited time.

## About project
This project is a 3D object simulator with all computer graphics algorithms implemented by myself. The project includes Phong and Gouraud lighting model, color reflection, Bezier curves etc.
In order to view an object, first you have to choose a file. You can choose between predefined options (sphere and torus) or choose your own .obj file in correct format.

![image](https://github.com/kubawini/3Dsimulatorv2/assets/93740269/bc6e16b8-f83d-4299-b7e1-05c6f4088c11)

After selecting a file, a window like this appears.

![image](https://github.com/kubawini/3Dsimulatorv2/assets/93740269/ea0804e6-e371-4d6f-ae40-938ff2b115d3)

You can modify following options on the sidebar:
* kd, km - factors regarding surface features,
* m - mirroring factor of the surface,
* z - light source height,
* "Staruj spiralę" button - it starts the lightsource movement,
* "Kolor obiektu" list view - choose color of your object,
* "Kolor światła" list view - choose color of your light source,
* "Włącz siatkę" checkbox - switch on grid displaying,
* "Interpolacja kolorów" radio button - Gouraud interpolation (worse quality, but more efficient),
* "Interpolacja wektorów" radio button - Phong interpolation (better quality, but less efficient),
* "Użyj tekstury" checkbox - display texture on object surface instead of plain color,
* "Użyj mapy wektorów" checkbox - modify surface with normal vector map (brick shape on the screenshot),
* "Włącz chmurkę" checkbox - show a cloud between object and light source (creates a shadow).

## Theory
A following lightmodel has been used

$`I = k_d * I_l * I_o * \cos(angle(N,L)) + k_s * I_l * I_o * \cos(angle(V,R))^m`$

where
* $k_d, k_s$ - factors regarding surface features,
* $I_l$ - light source color ((1,1,1) by default - white color),
* $I_o$ - object color,
* $L$ - versor towards light source,
* $N$ - normal vector,
* $m$ - mirroring factor of the surface,
* $V = [0,0,1]$,
* $R = 2 \cdot N \times L$.

The following triangulation algorithm is being used

![image](https://github.com/kubawini/3Dsimulatorv2/assets/93740269/d51c03a4-e349-4f47-9eb7-cb4a27e3616e)

## Technology
The UI for this program has been created using WPF framework. All algorithms have been created using C#.

## Prerequsiuties
Not all .obj file formats are supported. In order to look for some working examples, check resources folder.

