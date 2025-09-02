import pandas as pd

file_path = "FILENAME"
df = pd.read_excel(file_path, sheet_name="Table 1", header=1)

# Kolon isimlerini düzenle
df.columns = [
    "program_kodu", "program_adi", "puan_turu", "kontenjan",
    "yerlesen", "bos_kontenjan", "en_kucuk_puan", "en_buyuk_puan"
]

# Sayısal kolonları uygun tipe çevir
for col in ["kontenjan", "yerlesen", "bos_kontenjan", "en_kucuk_puan", "en_buyuk_puan"]:
    df[col] = pd.to_numeric(df[col], errors="coerce")

sql_lines = []

# CREATE TABLE
sql_lines.append("""
DROP TABLE IF EXISTS dgs_minmax;

CREATE TABLE dgs_minmax (
    program_kodu VARCHAR(20) PRIMARY KEY,
    program_adi TEXT,
    puan_turu VARCHAR(10),
    kontenjan INT,
    yerlesen INT,
    bos_kontenjan INT,
    en_kucuk_puan DECIMAL(8,5),
    en_buyuk_puan DECIMAL(8,5)
);
""")

# INSERT
for _, row in df.iterrows():
    sql_lines.append(f"""INSERT INTO dgs2023 (
        program_kodu, program_adi, puan_turu, kontenjan, yerlesen, bos_kontenjan, en_kucuk_puan, en_buyuk_puan
    ) VALUES (
        '{row['program_kodu']}', '{row['program_adi'].replace("'", "''")}', '{row['puan_turu']}', 
        {row['kontenjan'] if not pd.isna(row['kontenjan']) else 'NULL'}, 
        {row['yerlesen'] if not pd.isna(row['yerlesen']) else 'NULL'}, 
        {row['bos_kontenjan'] if not pd.isna(row['bos_kontenjan']) else 'NULL'}, 
        {row['en_kucuk_puan'] if not pd.isna(row['en_kucuk_puan']) else 'NULL'}, 
        {row['en_buyuk_puan'] if not pd.isna(row['en_buyuk_puan']) else 'NULL'}
    );
    """)

# Write
with open("dgs_minmax.sql", "w", encoding="utf-8") as f:
    f.write("\n".join(sql_lines))

print("SQL dosyası başarıyla oluşturuldu: dgs_minmax.sql")
