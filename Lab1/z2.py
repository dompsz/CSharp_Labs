from datetime import datetime
import time


class GPSPosition:
    def __init__(self, latitude, longitude, altitude=0.0):
        # set_ to tag as setters allowing for the comparison rules overwrite
        self.set_latitude(latitude)
        self.set_longitude(longitude)
        self.set_altitude(altitude)

    # getters
    def get_latitude(self):
        return self._latitude

    def get_longitude(self):
        return self._longitude

    def get_altitude(self):
        return self._altitude

    # setters
    def set_latitude(self, value):
        if not (-90 <= value <= 90):
            raise ValueError("Latitude must be between -90 and 90 degrees.")
        self._latitude = value

    def set_longitude(self, value):
        if not (-180 <= value <= 180):
            raise ValueError("Longitude must be between -180 and 180 degrees.")
        self._longitude = value

    def set_altitude(self, value):
        if not isinstance(value, (int, float)):
            raise ValueError("Altitude must be a number.")
        self._altitude = value

    # method
    def print_info(self):
        print(f"GPS Position: Latitude: {self.get_latitude()}, Longitude: {self.get_longitude()}, Altitude: {self.get_altitude()}")

    # str special method converts object to string for visual representation
    def __str__(self):
        return f"GPSPosition(latitude={self.get_latitude()}, longitude={self.get_longitude()}, altitude={self.get_altitude()})"

    # object comparison rules overwrite
    def __eq__(self, other):
        if not isinstance(other, GPSPosition):
            return False
        return (self.get_latitude() == other.get_latitude() and
                self.get_longitude() == other.get_longitude() and
                self.get_altitude() == other.get_altitude())


# inherits from GPSPosition and adds extra functionality.
class EnhancedGPSPosition(GPSPosition):
    def __init__(self, latitude, longitude, altitude=0.0, name="Unknown"):
        # Call parent class constructor
        super().__init__(latitude, longitude, altitude)
        self.name = name

    # Override (name)
    def print_info(self):
        print(f"EnhancedGPSPosition - {self.name}: Latitude: {self.get_latitude()}, Longitude: {self.get_longitude()}, Altitude: {self.get_altitude()}")

    def __str__(self):
        return f"EnhancedGPSPosition(name={self.name}, latitude={self.get_latitude()}, longitude={self.get_longitude()}, altitude={self.get_altitude()})"


# assigns timestamps to GPS positions and logs history
class GPSHistory:
    def __init__(self):
        self.history = []

    def add_position(self, position):
        if not isinstance(position, GPSPosition):
            raise ValueError("Only GPSPosition objects can be added.")
        timestamp = datetime.now()
        self.history.append((timestamp, position))

    # logs all records
    def print_history(self):
        print("GPS Position History:")
        for timestamp, pos in self.history:
            print(f"[{timestamp}] {pos}")

    def filter_by_time(self, start_time, end_time):
        return [(ts, pos) for ts, pos in self.history if start_time <= ts <= end_time]


def main():
    # objects
    pos1 = GPSPosition(52.2297, 21.0122, 100.0)  # Warsaw
    pos2 = GPSPosition(41.8919, 12.5113, 50.0)   # Rome

    # prints
    pos1.print_info()
    pos2.print_info()

    # object modifications
    pos1.set_latitude(52.4064)
    pos1.set_longitude(16.9252)
    pos1.set_altitude(80.0)

    # results
    print("\nAfter modification:")
    pos1.print_info()

    # Testing new classes
    pos3 = EnhancedGPSPosition(51.1079, 17.0385, 120.0, "Wrocław")
    pos4 = EnhancedGPSPosition(50.0647, 19.9450, 80.0, "Kraków")

    print("\nTesting EnhancedGPSPosition:")
    pos3.print_info()
    pos4.print_info()

    # Using GPSHistory to log positions with timestamps.
    gps_history = GPSHistory()
    gps_history.add_position(pos1)
    time.sleep(3) # delay for debug
    gps_history.add_position(pos2)

    print("\nGPS History:")
    gps_history.print_history()


if __name__ == "__main__":
    main()