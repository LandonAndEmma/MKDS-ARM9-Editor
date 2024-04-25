import base64
import io
import os
import tkinter as tk
import webbrowser
from tkinter import filedialog, messagebox

from PIL import Image, ImageTk

ARM9_BIN_PATH = None
ARM_VALUES = []
MUSIC_OFFSETS = {
    "Grand Prix Flyover": 0x153171,
    "Grand Prix Flyover (Figure-8 Circuit, GCN Luigi Circuit, Mario Circuit)": 0x153175,
    "Wario Stadium Flyover": 0x153179,
    "Battle Mode Flyover": 0x153181,
    "Boss Intro": 0x153185,
    "Figure-8 Circuit": 0x153189,
    "GCN Yoshi Circuit": 0x153191,
    "Cheep Cheep Beach": 0x153195,
    "Yoshi Falls": 0x153199,
    "Bowser Castle": 0x153201,
    "GBA Sky Garden": 0x153205,
    "Battle Mode": 0x153209,
    "New Reward": 0x153211,
    "Grand Prix Results": 0x153215,
    "Credits (50cc + 100cc)": 0x153219,
    "Wi-Fi Menu": 0x153221,
    "Multiplayer Menu": 0x153225,
    "Records Menu": 0x153229,
    "Game Intro": 0x153231,
    "Singleplayer Menu": 0x153235,
    "Grand Prix Flyover (Waluigi Pinball)": 0x15317D,
    "GCN Luigi Circuit": 0x15318D,
    "GCN Baby Park": 0x15319D,
    "N64 Moo Moo Farm": 0x1531A1,
    "N64 Frappe Snowland": 0x1531A5,
    "Delfino Square": 0x1531A9,
    "Airship Fortress": 0x1531AD,
    "Wario Stadium": 0x1531B1,
    "GCN Mushroom Bridge": 0x1531B5,
    "Peach Gardens": 0x1531B9,
    "Luigi's Mansion": 0x1531BD,
    "SNES Mario Circuit 1": 0x1531C1,
    "SNES Koopa Beach 2": 0x1531C5,
    "SNES Donut Plains 1": 0x1531C9,
    "SNES Choco Island 2": 0x1531CD,
    "GBA Peach Circuit": 0x1531D1,
    "GBA Luigi Circuit": 0x1531D5,
    "Shroom Ridge": 0x1531D9,
    "N64 Choco Mountain": 0x1531DD,
    "N64 Banshee Boardwalk": 0x1531E1,
    "DK Pass": 0x1531E5,
    "Desert Hills": 0x1531E9,
    "Waluigi Pinball": 0x1531ED,
    "Tick Tock Clock": 0x1531F1,
    "Mario Circuit": 0x1531F5,
    "Rainbow Road": 0x1531F9,
    "GBA Bowser Castle 2": 0x1531FD,
    "Boss Battle": 0x15320D,
    "Credits (150cc + Mirror)": 0x15321D,
}
ICON_BASE64 = """AAABAAEAEBAAAAEAIABoBAAAFgAAACgAAAAQAAAAIAAAAAEAIAAAAAAAAAQAAAAAAAAAAAAAAAAAAAAAAAAzIxH/MyIQ/zIjEP8AAQEAAQAAAAABAQAAAAAAAAAAAAAAAAAAAAAAAAAAAAEAAQAAAQAAMyMR/zIjEP8yIhD/NykW/0Y6Lf96c2//e3Jv/3tzb/97c2//e3Nu/3tzb/97cm//enNu/3pzb/97c27/e3Nu/3pyb/9GOiz/NygX/0U6I/+GhID/xMTO/+vr7f/39vf/9/b2//b39//29/f/9/f2//f39//29/b/9/b3/+vr7f/Fxc7/h4SB/0U7Iv9KQyn/SkIp/5eUk/92bmf/JSVP/wEJe/8ZFUf/GRVG/xkURv8ZFEb/AQh6/yQlT/92b2f/l5WS/0pDKf9KQij/VlMt/2NjMP9iYzH/MSAQ/xkVR/8BAcb/SEjd/25v3f9ub9z/SUnc/wEBxv8ZFUf/MCEQ/2JiMf9jYjH/V1Is/1hSQP9nY1f/Z2NW/0tDJf8+Mh3/Fxfe/4eG5f9UVOf/VFXn/4eH5P8WF97/PzId/0pDJf9nYlf/ZmJX/1lTQP90dFv/dXRa/3R0W/9lYkT/ZWJF/y0xuP+Agff/gID2/4GB9/+Bgff/LTC5/2VjRP9lYkX/dHVb/3R1W/90dFv/AAEBAAABAAAAAAAAoKGh/52clP/Awcf/VkpB/1IgX/9TIV//VkpB/8HAxv+cnJT/oKCh/wAAAAAAAAAAAAAAAAABAAAAAAAAAAAAAKKjqf+am57/vr++/3R+c/9LW2//Slpv/3R+cv++vr7/m5ue/6Ojqf8AAQAAAAABAAEAAQAAAAAAAAEAAAABAQDc3eD/3N3g/0tqnP9ERkf/TFlt/0xZbf9ERkf/S2qc/93d4f/d3eH/AAAAAAAAAAAAAAAAAAEAAAEBAAAAAQAAAAAAAAEAAABff6L/fp/M/2aIs/9niLP/hKnY/19+o/8AAAEAAAEAAAAAAAAAAAAAAAEAAAABAAAAAQAAAAEBAAABAQAAAAEADQ5h/4OjzP99fYD/fHyA/4KizP8NDmD/AAAAAAEBAAAAAQAAAAAAAAAAAQABAAAAAAEAAAAAAAABAQAAAAAAAAAEmP8NDmD/JC9u/yQvbv8MD2D/AAWY/wEAAAAAAQAAAAAAAAAAAAABAAAAAAAAAAEBAAAAAQAAAQABAAAAAAAAAMv/AQC9/xYasf8XGrD/AQC9/wAAy/8AAAAAAAEAAAABAQAAAAAAAAAAAAAAAQAAAAAAAQAAAAAAAAABAAAAAQABAERF7/+Tkuf/kpPn/0RF7/8AAAAAAAAAAAABAAAAAAAAAAEAAAAAAAABAAAAAQAAAAEBAAABAAEAAAAAAAEBAAAAAAAAWlv3/1ta9v8AAAEAAAEBAAABAAAAAAEAAAEAAAEAAAABAAEAH/gAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAOAHAADgBwAA4AcAAPgfAAD4HwAA+B8AAPgfAAD8PwAA/n8AAA=="""


def get_icon_from_base64(base64_string):
    icon_data = base64.b64decode(base64_string)
    icon = Image.open(io.BytesIO(icon_data))
    return ImageTk.PhotoImage(icon)


def read_file(filename):
    try:
        with open(filename, 'rb') as file:
            return file.read()
    except Exception as e:
        messagebox.showerror("Error",
                             f"Failed to open file: {filename}\n\nError: {e}")


def open_file():
    global ARM_VALUES, ARM9_BIN_PATH
    file_path = filedialog.askopenfilename(title="Select ARM9 Binary File",
                                           filetypes=(("Binary Files", "*.bin"), ("All Files", "*.*")))
    if file_path:
        ARM9_BIN_PATH = file_path
        if os.path.getsize(ARM9_BIN_PATH) == 0:
            messagebox.showerror("Error", "This is not an arm9.bin file.")
            return
        file_content = read_file(ARM9_BIN_PATH)
        if file_content:
            ARM_VALUES = [int(byte) for byte in file_content]
            if not ARM_VALUES:
                messagebox.showerror("Error", "This is not an arm9.bin file.")
            else:
                refresh_listbox()


def save_file(file_path=None):
    global ARM_VALUES, ARM9_BIN_PATH
    if not ARM9_BIN_PATH:
        messagebox.showerror("Error", "No file is opened to save.")
        return
    if file_path is None:
        file_path = ARM9_BIN_PATH
    try:
        with open(file_path, 'wb') as file:
            file.write(bytes(ARM_VALUES))
        messagebox.showinfo("Success", "Modified file saved successfully")
    except Exception as e:
        messagebox.showerror("Error", f"Failed to save modified file: {str(e)}")


def refresh_listbox():
    listbox.delete(0, tk.END)
    for i, (track, offset) in enumerate(MUSIC_OFFSETS.items()):
        listbox.insert(tk.END, f"{i}) {track} [{ARM_VALUES[offset]}]")


def open_popup():
    root.attributes("-disabled", True)
    popup_window = tk.Toplevel(root)
    popup_window.title("Change SEQ Value")
    popup_window.geometry("300x180")
    popup_window.resizable(False, False)
    popup_window.attributes("-topmost", True)
    popup_window.iconphoto(True, get_icon_from_base64(ICON_BASE64))
    seq_label = tk.Label(popup_window, text="Enter new SEQ value:")
    seq_label.pack()
    seq_entry = tk.Entry(popup_window)
    seq_entry.pack()

    def change_seq_value():
        nonlocal popup_window
        selected_item = listbox.get(listbox.curselection())
        selected_index = int(selected_item.split(")")[0])
        track_name = list(MUSIC_OFFSETS.keys())[selected_index]
        offset = MUSIC_OFFSETS[track_name]
        try:
            new_value = int(seq_entry.get())
            if 0 <= new_value <= 75:
                ARM_VALUES[offset] = new_value
                refresh_listbox()
                popup_window.destroy()
                root.attributes("-disabled", False)
                messagebox.showinfo("Success", f"SEQ value for {track_name} changed to {new_value}")
            else:
                popup_window.destroy()
                root.attributes("-disabled", False)
                messagebox.showerror("Error", "Invalid SEQ value. Value must be between 0 and 75.")
        except ValueError:
            popup_window.destroy()
            root.attributes("-disabled", False)
            messagebox.showerror("Error", "Invalid SEQ value. Value must be between 0 and 75.")

    seq_button = tk.Button(popup_window, text="Change SEQ Value", command=change_seq_value)
    seq_button.pack()
    cancel_button = tk.Button(popup_window, text="Cancel",
                              command=lambda: (popup_window.destroy(), root.attributes("-disabled", False)))
    cancel_button.pack()

    def close_popup_window():
        popup_window.destroy()
        root.attributes("-disabled", False)

    popup_window.protocol("WM_DELETE_WINDOW", close_popup_window)
    popup_window.transient(root)
    popup_window.grab_set()
    popup_window.mainloop()


def save_file_as():
    file_path = filedialog.asksaveasfilename(defaultextension=".bin",
                                             filetypes=(("Binary Files", "*.bin"), ("All Files", "*.*")))
    if file_path:
        save_file(file_path)


def open_help():
    messagebox.showinfo("Help",
                        "This program allows you to edit the music track SEQ IDs in the arm9.bin file of Mario Kart DS.\n\n"
                        "1. To get started, go to File > Open and select the arm9.bin file you want to edit.\n\n"
                        "2. Once the file is opened, click on a track in the list to change its SEQ ID.\n\n"
                        "3. After making changes, go to File > Save to save the modified file.\n\n"
                        "Original Code: Ermelber, Yami, MkDasher\n"
                        "Fixed and made into a Python GUI app by Landon & Emma")


def open_repository():
    webbrowser.open_new("https://github.com/LandonAndEmma/MKDS-ARM9-Music-Editor")


def on_closing():
    if not ARM9_BIN_PATH or messagebox.askokcancel("Quit", "Do you want to quit?"):
        root.destroy()


root = tk.Tk()
root.title("Mario Kart DS ARM9 Music Table Editor")
root.geometry("600x400")
root.iconphoto(True, get_icon_from_base64(ICON_BASE64))
menubar = tk.Menu(root)
file_menu = tk.Menu(menubar, tearoff=0)
file_menu.add_command(label="Open", command=open_file)
file_menu.add_command(label="Save", command=save_file)
file_menu.add_command(label="Save As", command=save_file_as)
menubar.add_cascade(label="File", menu=file_menu)
help_menu = tk.Menu(menubar, tearoff=0)
help_menu.add_command(label="Help", command=open_help)
help_menu.add_command(label="Repository", command=open_repository)
menubar.add_cascade(label="Help", menu=help_menu)
root.config(menu=menubar)
main_frame = tk.Frame(root)
main_frame.pack(fill=tk.BOTH, expand=True)
label = tk.Label(main_frame, text="Select a track to change the SEQ value:")
label.pack()
listbox = tk.Listbox(main_frame)
listbox.pack(fill=tk.BOTH, expand=True)


def on_listbox_select(event):
    selected_index = listbox.curselection()
    if selected_index:
        open_popup()


listbox.bind("<<ListboxSelect>>", on_listbox_select)
root.protocol("WM_DELETE_WINDOW", on_closing)
root.mainloop()
