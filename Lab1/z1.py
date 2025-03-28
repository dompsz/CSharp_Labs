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


if __name__ == "__main__":
    main()
