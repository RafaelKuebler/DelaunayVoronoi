# C# Delaunay triangulation + Voronoi Diagram

Данный проект был мною оптиммизирован, был добавлен новый функционал:
* Оптимизация на 330+% (в некоторых местах до 860%)
* Была заменена стандартная медленная библиотека рисования WPF на быструю библиотеку Lightning(В демо версии), для данного варианта была создана отдельная ветка- Lightning
* Был добавел новый функционал для построения точек в круге
* Был добавел новый функционал для построения точек методом распределения Гаусса
* Возможность через интерфейс задать кол-во точек для построения

<img alt="Delaunay triangulation and Voronoi diagram for 5000 points" src="screenshots/RandomSimple.png" width="700">
<img alt="Delaunay triangulation and Voronoi diagram for 5000 points" src="screenshots/Guassian.png" width="700">
<img alt="Delaunay triangulation and Voronoi diagram for 5000 points" src="screenshots/Circle.png" width="700">
