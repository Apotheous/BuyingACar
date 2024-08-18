https://github.com/user-attachments/assets/77d319d9-910b-40f9-9b00-fdc5b4e9dfee


* Project Overview
This project is designed to allow players to purchase and drive various cars. Each car has customizable attributes, including:

-Torque

-Maximum Speed

-Damaged Parts

-Painted Parts

-Suspension

-Wheel Camber

-Price

The initial price of each car is set at 50,000 gold. Players can negotiate to lower the price, which will be recalculated based on the car's specific attributes. If the new price is still unsatisfactory and the car's condition is sufficiently poor, there may be further opportunities for a price reduction.

Once a car is purchased, players can drive it around the map.

* In-Game Key Bindings
  
To interact with the seller, look at them and press the "X" key.
To enter and drive the purchased car, look at the vehicle and press the "E" key.

Driving Controls:

-"W" for Forward

-"S" for Reverse and Brake

-"A" for Left Turn

-"D" for Right Turn

* Dialogue and Car Management
  
-Dialogue Starter: You can customize the seller's dialogue using the Dialogue Starter. At any point, you can create External Functions to trigger specific actions during the conversation.
Car Store Management: Configure the cars available for sale in the Store Manager component, found under the Store GameObject, by modifying the MyCars array.
-Car Spawning: Cars will spawn in a defined arrangement behind the seller. Adjust the arrangement using the Spawn Offset variables.
-MrSellerCarPropsPanel: Displays the selected car's properties.
-CarPropsPanel: Each car's individual properties are displayed in the Canvas attached to the car, under the CarPropsPanel GameObject.

* Randomized Car Attributes
  
Car attributes are randomly generated within a specified range each time the game starts. This process is handled in the CarMain class and is invoked through the InitializeRandomValues() method within the Car class.

*Driving Mechanics

The CarController class is responsible for controlling the car's driving mechanics.

