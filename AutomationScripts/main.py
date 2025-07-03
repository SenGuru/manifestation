import csv
import sys

def process_csv(input_file, output_file):
    with open(input_file, 'r') as csv_file, open(output_file, 'w') as txt_file:
        csv_reader = csv.reader(csv_file)
        next(csv_reader)  # Skip the header row

        for row in csv_reader:
            if len(row) >= 2:
                table_name = row[0].strip()
                associations = row[1].strip()

                # Remove trailing colon and any text after it if the next item is empty
                associations = associations.rstrip(':')

                if associations:
                    command = f"ci cdf --for-table={table_name} --tables={associations}\n"
                else:
                    command = f"ci cdf --for-table={table_name} --tables=\n"

                txt_file.write(command)

if __name__ == "__main__":
    if len(sys.argv) != 3:
        print("Usage: python script.py <input_csv_file> <output_txt_file>")
        sys.exit(1)

    input_file = sys.argv[1]
    output_file = sys.argv[2]

    process_csv(input_file, output_file)
    print(f"Commands have been written to {output_file}")