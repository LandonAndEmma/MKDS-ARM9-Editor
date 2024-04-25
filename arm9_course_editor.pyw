import base64
import io
import os
import tkinter as tk
import webbrowser
from tkinter import filedialog, messagebox

from PIL import Image, ImageTk

ARM9_BIN_PATH = None
ARM_VALUES = []
COURSE_OFFSETS = {
    "Mission Run Boss Stage 1": (0x164998, 0x1649A3),
    "Mission Run Boss Stage 3": (0x1649A4, 0x1649AF),
    "Mission Run Boss Stage 4": (0x1649B0, 0x1649BB),
    "StaffRoll": (0x1649BC, 0x1649C7),
    "Mission Run Boss Stage 2": (0x1649C8, 0x1649D3),
    "Delfino Square": (0x1649D4, 0x1649DF),
    "DK Pass": (0x1649E0, 0x1649EB),
    "GBA Sky Garden": (0x1649EC, 0x1649F7),
    "Nintendo DS": (0x1649F8, 0x164A03),
    "Twilight House": (0x164A04, 0x164A0F),
    "Palm Shore": (0x164A10, 0x164A1B),
    "Tart Top": (0x164A1C, 0x164A27),
    "N64 Moo Moo Farm": (0x164A28, 0x164A33),
    "GCN Baby Park": (0x164A34, 0x164A3F),
    "test_circle (unused)": (0x164A40, 0x164A4B),
    "Yoshi Falls": (0x164A4C, 0x164A57),
    "Mario Circuit": (0x164A58, 0x164A67),
    "Cheep Cheep Beach": (0x164A68, 0x164A77),
    "Bowser Castle": (0x164A78, 0x164A87),
    "N64 Choco Mountain": (0x164A88, 0x164A97),
    "GCN Yoshi Circuit": (0x164A98, 0x164AA7),
    "old_mario_gc (unused)": (0x164AB8, 0x164AC7),
    "luigi_course (unused)": (0x164AC8, 0x164AD7),
    "dokan_course (unused)": (0x164AD8, 0x164AE7),
    "test1_course (unused)": (0x164AE8, 0x164AF7),
    "wario_course (unused)": (0x164AF8, 0x164B07),
    "GCN Luigi Circuit": (0x164B08, 0x164B17),
    "SNES Koopa Beach 2": (0x164B18, 0x164B27),
    "Tick Tock Clock": (0x164B28, 0x164B38),
    "Figure-8 Circuit": (0x164B38, 0x164B47),
    "Shroom Ridge": (0x164B48, 0x164B57),
    "Peach Gardens": (0x164B58, 0x164B67),
    "Desert Hills": (0x164B68, 0x164B76),
    "SNES Donut Plains 1": (0x164B78, 0x164B87),
    "GBA Luigi Circuit": (0x164B88, 0x164B97),
    "GCN Mushroom Bridge": (0x164B98, 0x164BA7),
    "SNES Choco Island 2": (0x164BA8, 0x164BB7),
    "N64 Block Fort": (0x164BB8, 0x164BC7),
    "Pipe Plaza": (0x164BC8, 0x164BD7),
    "StaffRollTrue": (0x164BD8, 0x164BE7),
    "donkey_course (unused)": (0x164BE8, 0x164BF7),
    "SNES Mario Circuit 1": (0x164BF8, 0x164C07),
    "GBA Bowser Castle 2": (0x164C08, 0x164C17),
    "GBA Peach Circuit": (0x164C18, 0x164C27),
    "N64 Frappe Snowland": (0x164C28, 0x164C37),
    "Rainbow Road": (0x164C38, 0x164C47),
    "Waluigi Pinball": (0x164C48, 0x164C57),
    "Wario Stadium": (0x164C58, 0x164C67),
    "Luigi's Mansion": (0x164C68, 0x164C77),
    "Arship Fortress": (0x164C78, 0x164C87),
    "N64 Banshee Boardwalk": (0x164C88, 0x164C97),
    "nokonoko_course (unused)": (0x164C98, 0x164CA7),
    "mini_block_course (unused)": (0x164CA8, 0x164CBB),
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
        messagebox.showerror("ERROR",
                             f"Failed to open file: {filename}\n\nError: {e}")


def open_file():
    global ARM_VALUES, ARM9_BIN_PATH
    file_path = filedialog.askopenfilename(title="Select ARM9 Binary File",
                                           filetypes=(("Binary Files", "*.bin"), ("All Files", "*.*")))
    if file_path:
        ARM9_BIN_PATH = file_path
        if os.path.getsize(ARM9_BIN_PATH) == 0:
            messagebox.showerror("ERROR", "This is not an arm9.bin file.")
            return
        file_content = read_file(ARM9_BIN_PATH)
        if file_content:
            ARM_VALUES = [int(byte) for byte in file_content]
            if not ARM_VALUES:
                messagebox.showerror("ERROR", "This is not an arm9.bin file.")
            else:
                refresh_listbox()


def save_file(file_path=None):
    global ARM_VALUES, ARM9_BIN_PATH
    if not ARM9_BIN_PATH:
        messagebox.showerror("ERROR", "No file is opened to save.")
        return
    if file_path is None:
        file_path = ARM9_BIN_PATH
    try:
        with open(file_path, 'wb') as file:
            file.write(bytes(ARM_VALUES))
        messagebox.showinfo("Success", "Modified file saved successfully")
    except Exception as e:
        messagebox.showerror("ERROR", f"Failed to save modified file: {str(e)}")


def refresh_listbox():
    listbox.delete(0, tk.END)
    for track, offsets in COURSE_OFFSETS.items():
        name_offset, size_offset = offsets
        track_name_bytes = bytes(ARM_VALUES[name_offset:size_offset])
        track_name = track_name_bytes.decode('utf-8', errors='replace').rstrip('\x00')
        listbox.insert(tk.END, f"{track} [{track_name}]")


def open_popup():
    root.attributes("-disabled", True)
    popup_window = tk.Toplevel(root)
    popup_window.title("Change File Name")
    popup_window.geometry("300x180")
    popup_window.resizable(False, False)
    popup_window.iconphoto(True, get_icon_from_base64(ICON_BASE64))
    course_label = tk.Label(popup_window, text="Enter new file name:")
    course_label.pack()
    file_entry = tk.Entry(popup_window)
    file_entry.pack()

    def change_file_name():
        nonlocal popup_window
        selected_index = listbox.curselection()
        if not selected_index:
            messagebox.showerror("ERROR", "No course selected.")
            return
        selected_index = int(selected_index[0])
        course_name = list(COURSE_OFFSETS.keys())[selected_index]
        name_offset, size_offset = COURSE_OFFSETS[course_name]
        new_name = file_entry.get().encode('utf-8')
        if not new_name:
            messagebox.showwarning("WARNING",
                                   f"You have made the new name blank, ONLY DO THIS IF YOU DON'T WANT THE SPECIFIC COURSE TO LOAD!")
        else:
            block_size = size_offset - name_offset
            if len(new_name) > block_size:
                messagebox.showerror("ERROR", f"New file name exceeds block size ({block_size} bytes).")
                return
            new_name += b'\x00' * (block_size - len(new_name))
            ARM_VALUES[name_offset:size_offset] = new_name
            refresh_listbox()
            popup_window.destroy()
            root.attributes("-disabled", False)
            messagebox.showinfo("Success", f"File name for {course_name} changed to {new_name.decode('utf-8')}")

    seq_button = tk.Button(popup_window, text="Change Course Name", command=change_file_name)
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
                        "This program allows you to edit the course file name referenced in the arm9.bin file of Mario Kart DS.\n\n"
                        "1. To get started, go to File > Open and select the arm9.bin file you want to edit.\n\n"
                        "2. Once the file is opened, click on a course in the list to change its file name reference.\n\n"
                        "3. After making changes, go to File > Save to save the modified file.\n\n"
                        "Made by Landon & Emma")


def open_repository():
    webbrowser.open_new("https://github.com/LandonAndEmma/MKDS-ARM9-Course-Name-Reference-Editor")


def on_closing():
    if not ARM9_BIN_PATH or messagebox.askokcancel("Quit", "Do you want to quit?"):
        root.destroy()


root = tk.Tk()
root.title("Mario Kart DS ARM9 Course Name Reference Editor")
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
label = tk.Label(main_frame, text="Select a course to change the referenced file name:")
label.pack()
listbox = tk.Listbox(main_frame)
listbox.pack(fill=tk.BOTH, expand=True)
listbox.bind("<<ListboxSelect>>", lambda event: open_popup())
root.protocol("WM_DELETE_WINDOW", on_closing)
root.mainloop()
